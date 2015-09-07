using UnityEngine;
using System.Collections;

public class ConversationManager : MonoBehaviour 
{
	// Nodes are the pieces of this conversation. We will run them in the order that the children of this gameobject is placed
	private Node[] all_nodes;
	private int cur_node = 0;
	private bool active = false;	// Set to true when this is current conversation

	void Start () 
	{
		all_nodes = this.GetComponentsInChildren<Node>();
	}


	// Simply start the first node to get this conversation going
	public void Start_Conversation()
	{
		// Set this conversation as active in the scene manager
		SceneManager.current_conversation = this;

		active = true;
		Start_Node();	// Start the first conversation node
	}


	// Move onto the next node.
	// If there is no next node, this conversation is over and we should move on to the next one or load a different scene.
	public void Start_Next_Node()
	{
		cur_node++;

		if (cur_node < all_nodes.Length)
			Start_Node();
		else
		{
			Finish_Conversation();
		}
	}
	// Runs the current node
	void Start_Node()
	{
		all_nodes[cur_node].Run_Node();
	}


	// Returns the current node in the conversation
	public Node Get_Current_Node()
	{
		return all_nodes[cur_node];
	}


	// Destroys this game object.
	// Be sure to have added a StartConversationNode or LoadSceneNode before the conversation is over,
	// else nothing will happen!
	public void Finish_Conversation()
	{
		GameObject.Destroy(this.gameObject);
	}

	
	// Update is called once per frame
	void Update () {
		// Check for user input
		if (active && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0)))
		{
			all_nodes[cur_node].Button_Pressed();
		}
	}
}
