using UnityEngine;
using System.Collections;

public class UnitButton : MonoBehaviour 
{
    public string button_name = "FacingButton";


	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}


    void OnMouseDown()
    {
        Debug.Log(button_name + " clicked");
    }
}
