using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeUnitStats : MonoBehaviour {
    public string unitPrefab;
    public Text unitNameText;
    public Text unitDescription;
    public Text health;
    public Text defence;
    public Text rangedDefence;
    public Text damage;
    public Text piercingDamage;
    public Text movement;
    public Slider healthBar;
    public Slider defenceBar;
    public Slider rangedDefenceBar;
    public Slider damageBar;
    public Slider piercingDamageBar;
    public Slider movementBar;

    public static ChangeUnitStats unitPanel;

    // Use this for initialization
    void Start () {
        changeStats("Hoplite");
        unitPanel = this;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    
	}

    public void changeStats(string unitName)
    {
        GameObject unit = (GameObject)Resources.Load("Battles/Units/" + unitName);
        Unit unitStats = unit.GetComponent<Unit>();
        unitNameText.text = unitStats.u_name;
        unitDescription.text = "<i>" + unitStats.u_description + "</i>";
        health.text = "Health:" + unitStats.maximum_health;
        healthBar.value = 1;
        defence.text = "Defence:" + unitStats.normal_defence;
        defenceBar.value = unitStats.normal_defence;
        rangedDefence.text = "Ranged Defence:" + unitStats.normal_ranged_defence;
        rangedDefenceBar.value = unitStats.normal_ranged_defence;
        damage.text = "Damage: " + unitStats.GetDamage();
        damageBar.value = unitStats.GetDamage() / 200.0f;
        piercingDamage.text = "Piercing Damage: " + unitStats.GetPiercingDamage();
        piercingDamageBar.value = unitStats.GetPiercingDamage() / 200.0f;
        movement.text = "Movement Speed: " + unitStats.GetMovement();
        movementBar.value = (float)unitStats.GetMovement() / 10.0f;
        unitPrefab = unitName;
    }
}
