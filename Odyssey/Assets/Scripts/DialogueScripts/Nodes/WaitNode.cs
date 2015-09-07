using UnityEngine;
using System.Collections;

public class WaitNode : Node {

	public float wait_for_seconds;

	public override void Run_Node()
	{
		StartCoroutine(Wait(wait_for_seconds));
	}
	
	
	public override void Finish_Node()
	{
		base.Finish_Node();
	}


	public IEnumerator Wait(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Finish_Node();
	}
}
