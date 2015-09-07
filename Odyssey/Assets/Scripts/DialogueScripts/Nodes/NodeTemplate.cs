using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Not used in real code. Merely a template to copy and paste from when creating new nodes.
public class NodeTemplate : Node 
{
	public override void Run_Node()
	{
		//StartCoroutine();
	}


	public override void Button_Pressed()
	{
		Finish_Node();
	}


	public override void Finish_Node()
	{
		StopAllCoroutines();
		
		base.Finish_Node();
	}
}
