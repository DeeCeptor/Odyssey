using UnityEngine;
using System.Collections;

// Manages favor and god abilities
public class GodsManager : MonoBehaviour
{
    [HideInInspector]
    public static GodsManager gods_manager;

    public int favour_remaining; // Favor is used to activate abilities.


	void Awake ()
    {
        gods_manager = this;
    }
    void Start ()
    {
        SetFavourRemainingText();
    }
	

    public void ModifyFavour(int amount)
    {
        favour_remaining = Mathf.Max(favour_remaining + amount, 0);   // Can't have negative favor
        SetFavourRemainingText();
    }
    // Returns true if there's enough favor_remaining to cast this ability
    public bool CanCast(int cost)
    {
        return (-cost + favour_remaining >= 0) ;
    }

    public void SetFavourRemainingText()
    {
        PlayerInterface.player_interface.favour_remaining_text.text = "Favour remaining: " + favour_remaining;
    }
}
