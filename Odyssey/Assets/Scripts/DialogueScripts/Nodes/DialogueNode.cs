using UnityEngine;
using System.Collections;

public class DialogueNode : Node {
	
	public string actor;	// Name of actor to use for talking
	public string textbox_title;	// Name to be placed at top of textbox. Ex: 'Bob'
	[TextArea(3,10)]
	public string text;
	public bool clear_text_after = false;

	private bool done_printing = false;

	public override void Run_Node()
	{
		UIManager.ui_manager.dialogue_text_panel.text = "";
		UIManager.ui_manager.speaker_text_panel.text = textbox_title;
		StartCoroutine(animateText(text, VNProperties.delay_per_character));

		// If the actor field is filled in and the actor is present on the scene
		if (actor != "" && ActorManager.Get_Actor(actor) != null)
		{
			Actor speaker = ActorManager.Get_Actor(actor);

			// Slightly darken all other actors
			ActorManager.Darken_All_Actors_But(speaker);

			// Lighten this actor to show this one is talking
			speaker.Lighten();
		}
	}

	public override void Button_Pressed()
	{
		if (done_printing)
			Finish_Node();
		else
		{
			// Show all the text in this dialogue. This lets fast readers click to show all the dialog
			// instead of waiting it all to appear. User must click again to finish this dialogue piece.
			done_printing = true;
			StopAllCoroutines();
			UIManager.ui_manager.dialogue_text_panel.text = text;
		}
	}
	
	public override void Finish_Node()
	{
		StopAllCoroutines();

		if (clear_text_after)
		{
			UIManager.ui_manager.speaker_text_panel.text = "";
			UIManager.ui_manager.dialogue_text_panel.text = "";
		}

		// Record what was said in the log so players can go back and read anything they missed
		SceneManager.scene_manager.Add_To_Log(textbox_title, text);

		base.Finish_Node();
	}


	// Prints the text to the UI manager's dialogue text one character at a time.
	// It waits time_between_characters seconds before adding on the next character.
	public IEnumerator animateText(string strComplete, float time_between_characters) 
	{
		int i = 0;
		while(i < strComplete.Length)
		{
			UIManager.ui_manager.dialogue_text_panel.text += strComplete[i++];
			yield return new WaitForSeconds(time_between_characters);
		}
		done_printing = true;
	}


	void Start () {

	}
	
	void Update () {
	
	}
}
