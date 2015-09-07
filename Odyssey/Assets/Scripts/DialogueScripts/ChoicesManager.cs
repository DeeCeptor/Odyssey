using UnityEngine;
using System.Collections;

// Script containing functions called by the Choices buttons
public class ChoicesManager : MonoBehaviour 
{
	// Starts the designated conversation, and stops the current conversation
	public void Change_Conversation(ConversationManager conversation_to_start)
	{
		SceneManager.current_conversation.Finish_Conversation();
		conversation_to_start.Start_Conversation();
	}


	public void Continue_Conversation()
	{
		SceneManager.current_conversation.Get_Current_Node().Finish_Node();
	}


	public void Write(string conversation_to_switch_to)
	{
		Debug.Log("Test " + conversation_to_switch_to);
	}


	void Start () 
	{
	
	}


	void Update () 
	{
	
	}
}
