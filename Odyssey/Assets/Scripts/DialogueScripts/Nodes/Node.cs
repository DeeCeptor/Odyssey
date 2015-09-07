using UnityEngine;
using System.Collections;

// Base class of Nodes, which make up all conversations
public class Node : MonoBehaviour {

	// Called when the node is run. Override this method for inheriting classes of Node
	public virtual void Run_Node()
	{

	}


	// Called when the node is finished executing. Override this method for inheriting classes of Node.
	// Overridden methods should call base.Finish_Node() after they're done executing.
	public virtual void Finish_Node()
	{
		// Runs the next node in the conversation
		this.GetComponentInParent<ConversationManager>().Start_Next_Node();
	}


	// User clicked, or hit enter. Let the node handle this in its own way. Override this method for inheriting classes of Node
	public virtual void Button_Pressed()
	{

	}
}
