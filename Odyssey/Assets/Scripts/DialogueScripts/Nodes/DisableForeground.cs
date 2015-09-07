using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Disables the foreground
// Used so foreground image does not stop the user from interacting with the UI.
public class DisableForeground : Node 
{
	
	public override void Run_Node()
	{
		UIManager.ui_manager.foreground.gameObject.SetActive(false);
		Finish_Node();
	}
	
	
	public override void Finish_Node()
	{
		StopAllCoroutines();
		
		base.Finish_Node();
	}
}
