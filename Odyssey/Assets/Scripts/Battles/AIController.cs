using UnityEngine;
using System.Collections.Generic;

public class AIController 
{
    Faction faction;

    public AIController(Faction AI_faction)
    {
        faction = AI_faction;
    }


    public void Do_Turn()
    {

        // By this point, all units should have their movable tiles updated
        // This AI will evaluate each tile the AI can move to, and pick the best one
        foreach (Unit unit in faction.units)
        {
            Hex best_hex = unit.location;   // Start off assuming where they are is the best location
            best_hex.hex_score = EvaluateHexScore(unit, best_hex);
            Debug.Log(best_hex.hex_score);

            // Go through each unit, evaluate every hex it could move to
            foreach (Hex hex in unit.tiles_I_can_move_to)
            {
                hex.hex_score = EvaluateHexScore(unit, hex);

                if (hex.hex_score > best_hex.hex_score)
                    best_hex = hex;
            }

            // We've gone through each hex. Move to that hex
            Debug.Log("Moving unit to " + best_hex.coordinate + ", " + best_hex.hex_score);
            unit.PathTo(best_hex);

            // Now that we know where we're ending up, evaluate the best target we can attack from there
            Unit target = null;
            float best_target_score = 0;
            foreach (Unit enemy in unit.owner.GetAllEnemyUnits())
            {
                // Check if we're in range. If we're not in range, we can't attack the unit
                if (HexMap.hex_map.InRange(unit.location, enemy.location, unit.GetRange()))
                {
                    float cur_score = enemy.CalculateDamage(unit);

                    if (cur_score > best_target_score)
                    {
                        // This is our best target to attack so far. Record it.
                        target = enemy;
                        best_target_score = cur_score;
                    }
                }
            }

            // If there's a suitable target, have the unit attack it once it gets to the right hex
            if (best_target_score > 0)  
            {
                unit.attack_target = target;
            }
        }

        Debug.Log("Finished AI turn");
        BattleManager.battle_manager.EndTurn();
    }

    
    // Returns the value of this hex to the AI player. High value is good.
    public float EvaluateHexScore(Unit unit, Hex hex)
    {
        // Add the bonuses this hex gives
        float hex_score = hex.defense_score;

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
            score = Mathf.Max(score, target_hex.occupying_unit.CalculateDamage(cur_unit));
        }

        return score;
    }


    // See how many allies there are around this hex
    public float AllyScoreOnHex(Unit cur_unit, Hex hex)
    {
        float score = 0;

        // Find all hexes with allies on them
        List<Hex> potential_targets = HexMap.hex_map.HexesWithinRangeContainingAllies(hex, cur_unit.GetRange(), cur_unit.owner);
        foreach (Hex target_hex in potential_targets)
        {
            if (target_hex.occupying_unit != cur_unit)
                score += 0.1f;
        }

        return score;
    }
}
