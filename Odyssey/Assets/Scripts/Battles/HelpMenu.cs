using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HelpMenu : MonoBehaviour
{
    public Text help_text;

    private string deployment =
        "You can view the map before the battle begins. You may deploy units on non-darkened hexes. You may study the battlefield for as long as you like. Click the 'Start Battle' button to begin the battle."
        + "\n\nUnits that you've acquired will populate the menu at the top of the screen. Click on a unit and then click on a non-darkened hex to deploy that unit."
        + "\n\nMost units are deployed in squads. It takes multiple individuals to form squads. For example, you will need 5 Hoplites to form a full Hoplite unit to use in battle. Wounded units may not be used in battle. "
        ;

    private string controlling_units = 
        "Mouse over a unit to view its attributes."
        + "\n\nLeft click on a unit that you control to select it. Hexes that the unit can move to are highlighted in blue. Enemies it can attack from the current hex, or the hex the mouse is over are highlighted in red."
        + "\n\nUnits may move once, and then attack. Units may not attack then move."
        + "\n\nRight click ona hex to move to it, and right click on an enemy to attack it. Remember the enemy may attack back due to it facing your unit!"
        + "\n\nAbilities are displayed above the selected unit. Mouse over an ability to read a description of it."
        ;

    private string facing =
        "The highlighted edges of hexes represent which way the unit is facing. Press the 'F' key once and then use the mouse to change the facing of your unit. Press the 'F' key again to stop rotating your unit."
        + "\n\nUnit facing is extremely important. If a defending unit is facing its attacker, it will counterattack against the agressor if possible. This makes flanking extremely important. Ranged units do not normally counterattack, except for peltasts."
        + "\n\nSome units receive damage bonuses when attacking a unit that is not fdacing them. Cavalry gain a +50% damage bonus when flanking enemies."
        ;

    private string favour =
        "The gods watch battles carefully, taking glee in the bloodshed and suffering of mortals. Their influence can often be felt on the battlefield, inspiring units to perform extraordinary feats."
        + "\n\nYour favour with the gods is displayed along the top of screen. This favour can be used by unit abilities to give you an advantage in battle."
        + "\n\nYour Piety skill increases the maximum favour you can hold. Killing enemies in combat grants you favour."
        ;

    private string abilities =
        "Every unit has both passive and active abilities."
        + "\n\nPassive abilities are abilities that do not need to be activated. For example, Hoplites gain defence when other Hoplites are adjacent to them."
        + "\n\nActive abilities need to be activated. Select your units to view their abilities above the unit."
        + "\n\nActive abilities often cost God Favour. You cannot cast these abilities if you do not have the favour. These abilities can only be cast once per battle."
        + "\n\nActive Abilities often only last for 1 turn."
        ;

    private string unit_stats =
        "Units statistics are displayed in the bottom left panel."
        + "\n\nHealth: When a units health reaches 0, it is removed from the battle. Health also effects the damage of a unit."
        + "\n\nDefence: Defence is measured as a percentage. Defence is used for melee attacks. 50% defence means all melee attacks will do 50% less normal damage. Defence does not block Piercing Damage."
        + "\n\nRanged Defence: Same as defence, but used against ranged attacks. Ranged defence does not block ranged damage."
        + "\n\nDamage: How much damage is infliced by this unit. Damage is effected by the targets' defence. Damage is correlated to health. A unit at half of its maximum Health will do half of its normal damage."
        + "\n\nPiercing Damage: Piercing damage ignores the defence of the target. Piercing damage is still correlated to unit health. A unit at half of its maximum Health will do half of its normal piercing damage."
        + "\n\nMovement Speed: How far the unit can move. A normal hex takes 1 movement point to traverse. Some hexes require more movement to traverse."
        ;

    private string terrain =
        "Gaia does not play an active role in battle, but her terrain may swing the course of battle."
        + "\n\nMouse over hexes to view their effects on the bottom right panel of the screen. Units gain these effects when they move onto that hex."
        + "\n\nFor example, forests give units significant ranged defence, cust take 2 movement to move into."
        ;

    private string overworld_controls =
        "Something something left click draw"
        ;
    private string food_water =
    "Something something left click draw"
    ;
    private string exploration =
    "Something something left click draw"
    ;
    private string objective =
    "Something something left click draw"
    ;
    private string gods =
    "Something something left click draw"
    ;
    private string heroes =
    "Something something left click draw"
    ;

    public void SetText(string category)
    {
        switch (category)
        {
            case "Deployment":
                help_text.text = deployment;
                break;
            case "Controls":
                help_text.text = controlling_units;
                break;
            case "Facing":
                help_text.text = facing;
                break;
            case "God Favour":
                help_text.text = favour;
                break;
            case "Abilities":
                help_text.text = abilities;
                break;
            case "Unit Stats":
                help_text.text = unit_stats;
                break;
            case "Terrain":
                help_text.text = terrain;
                break;
            case "OverworldControls":
                help_text.text = overworld_controls;
                break;
            case "Food & Water":
                help_text.text = food_water;
                break;
            case "Exploration":
                help_text.text = exploration;
                break;
            case "Objective":
                help_text.text = objective;
                break;
            case "Gods":
                help_text.text = gods;
                break;
            case "Heroes":
                help_text.text = heroes;
                break;
        }
    }
}
