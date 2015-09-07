using UnityEngine;
using System.Collections;

public class ChangeConversationNode : Node 
{
	public ConversationManager conversation_to_start;

	public override void Run_Node()
	{
		conversation_to_start.Start_Conversation();
		this.transform.GetComponentInParent<ConversationManager>().Finish_Conversation();
	}
	
	
	public override void Button_Pressed()
	{

	}
	
	
	public override void Finish_Node()
	{
		StopAllCoroutines();
		
		base.Finish_Node();
	}
}
