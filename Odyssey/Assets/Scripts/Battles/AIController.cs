using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using System;

public class AIController 
{
    Faction faction;
    AI_Turn_Thread AI_turn;
    public bool done_AI_turn = false;

    public AIController(Faction AI_faction)
    {
        faction = AI_faction;
        AI_turn = new AI_Turn_Thread(faction, this);
    }


    public void Do_Turn()
    {
        // Create a thread to do the work for this turn
        Debug.Log("Creating thread for enemy turn");
        done_AI_turn = false;
        Thread AI_thread = new Thread(new ThreadStart(AI_turn.Execute_AI_Turn));
        AI_thread.Start();
    }

}

public class AI_Turn_Thread
{
    Faction faction;
    AIController controller;

    public AI_Turn_Thread(Faction cur_faction, AIController cur_controller)
    {
        faction = cur_faction;
        controller = cur_controller;
    }

    public void Execute_AI_Turn()
    {
        controller.done_AI_turn = false;

        // By this point, all units should have their movable tiles updated
        // This AI will evaluate each tile the AI can move to, and pick the best one
        List<Unit> units_needing_turns = new List<Unit>();
        units_needing_turns.AddRange(faction.units);
        foreach (Unit unit in units_needing_turns)
        {
            try
            {
                unit.attack_target = null;
                Hex best_hex = unit.location;   // Start off assuming where they are is the best location

                if (!unit.has_moved && !unit.has_attacked)
                {
                    //////// MOVING //////////
                    best_hex.hex_score = EvaluateHexScore(unit, best_hex);

                    // Evaluate every hex this unit can move to
                    foreach (Hex hex in unit.tiles_I_can_move_to)
                    {
                        hex.hex_score = EvaluateHexScore(unit, hex);

                        if (hex.hex_score > best_hex.hex_score)
                            best_hex = hex;
                    }

                    // We've gone through each hex. Move to that hex
                    unit.PathTo(best_hex);
                }


                if (!unit.has_attacked)
                {
                    /////// ATTACKING ////////
                    // Now that we know where we're ending up, evaluate the best target we can attack from there
                    Unit target = null;
                    float best_target_score = 0;
                    int closest_distance = 999;
                    Unit closest_enemy = null;
                    foreach (Unit enemy in unit.owner.GetAllEnemyUnits())
                    {
                        //////// FACING ////////////
                        // Check if this is the closest enemy, so we can face towards them
                        int distance = HexMap.hex_map.DistanceBetweenHexes(unit.location.coordinate, enemy.location.coordinate);
                        if (distance < closest_distance)
                        {
                            // This is the closest enemy we've evaluated. 
                            closest_enemy = enemy;
                            closest_distance = distance;
                        }


                        // Check if we're in range. If we're not in range, we can't attack the unit
                        if (distance <= unit.GetRange())
                        {
                            float cur_score = enemy.CalculateDamage(unit, best_hex);

                            if (cur_score > best_target_score)
                            {
                                // This is our best target to attack so far. Record it.
                                target = enemy;
                                best_target_score = cur_score;
                            }
                        }
                    }

                    // ROTATION
                    if (closest_enemy != null)
                    {
                        unit.SetDesiredRotationTowards(best_hex.world_coordinates, closest_enemy.location.world_coordinates);//?? 
                    }

                    // If there's a suitable target, have the unit attack it once it gets to the right hex
                    if (best_target_score > 0)
                    {
                        //Debug.Log(unit.u_name + " attacking " + target.u_name);
                        unit.attack_target = target;
                    }
                }              
            }
            catch (Exception e)
            { Debug.Log("Exception executing " + faction.faction_name + " " + unit.u_name + "'s turn: " + e.Message);    }

            // Wait until the unit is done moving before moving onto the next unit
            // This is done so the collection of enemy units is not modified by others attacking and killing enemies while trying to evaluate them
            if (unit.attack_target != null)
            {
                Thread.Sleep(500);  // Have a delay after attacking to give the lists a chance to update
                while (unit.is_moving)
                {
                    Thread.Sleep(100);
                }
            }
        }

        Debug.Log("Finished AI turn");
        controller.done_AI_turn = true;
        //BattleManager.battle_manager.EndTurn(); //??
    }


    // Returns the value of this hex to the AI player. High value is good.
    public float EvaluateHexScore(Unit unit, Hex hex)
    {
        // Add the bonuses this hex gives
        float hex_score = hex.HexTerrainScoreForUnit(unit);

        // Add score based on the damage we can do to enemies from this hex
        hex_score += EnemyScoreOnHex(unit, hex);

        // Add score based on the number of allies around this hex
        hex_score += AllyScoreOnHex(unit, hex);

        return hex_score;
    }


    // Check how much damage we can do to an enemy from this hex
    public float EnemyScoreOnHex(Unit cur_unit, Hex hex)
    {
        float score = 0;

        // Check if we can attack a unit from this hex
        List<Hex> potential_targets = HexMap.hex_map.HexesWithinRangeContainingEnemies(hex, cur_unit.GetRange(), cur_unit.owner);
        foreach (Hex target_hex in potential_targets)
        {
            score = Mathf.Max(score, target_hex.occupying_unit.CalculateDamage(cur_unit, target_hex));

            // Increase the score of this hex if we can attack them without being counterattacked
            if (cur_unit.attacks_are_counterable)
            {
                if (!target_hex.occupying_unit.counter_attacks || !target_hex.occupying_unit.IsFacing(hex))    //??
                {
                    score *= cur_unit.flanking_factor;
                }
            }
        }

        // If we can't attack someone from this hex, 
        if ((score == 0 && cur_unit.offensive_AI_score > 0))
        {
            float closest_enemy_distance = 100000;
            // Find the closest enemy unit, and make this score higher the closer we are to it
            foreach (Unit enemy in cur_unit.owner.GetAllEnemyUnits())
            {
                int dist = HexMap.hex_map.DistanceBetweenHexes(hex.coordinate, enemy.location.coordinate);

                if (dist < closest_enemy_distance)
                    closest_enemy_distance = dist;
            }

            // Move towards enemies if we have no target
            if (score == 0 && closest_enemy_distance < 100000)
                score = (20f - closest_enemy_distance) / 5f;


            // This section is for ranged units. They should move to their max range to stay away from enemy melee
            // If the value is positive, that means that this unit is closer than this unit's range. We don't like that, so try to move to max range to stay alive
            if (cur_unit.GetRange() > 1)
            {
                float distance_versus_range = closest_enemy_distance - cur_unit.GetRange();
                if (distance_versus_range < 0)
                {
                    score += distance_versus_range;
                }
            }
        }

        return score;
    }


    // See how many allies there are around this hex
    public float AllyScoreOnHex(Unit cur_unit, Hex hex)
    {
        float score = 0;

        if (cur_unit.ally_grouping_score > 0)
        {
            // Find all hexes with allies on them
            List<Hex> potential_targets = HexMap.hex_map.HexesWithinRangeContainingAllies(hex, cur_unit.GetRange(), cur_unit.owner);
            foreach (Hex target_hex in potential_targets)
            {
                if (target_hex.occupying_unit != cur_unit)
                    score += cur_unit.ally_grouping_score;
            }
        }

        return score;
    }

}
