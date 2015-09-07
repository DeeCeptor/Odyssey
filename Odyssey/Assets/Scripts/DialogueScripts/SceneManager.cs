using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour 
{
	public ConversationManager starting_conversation;

	[HideInInspector]
	public static ConversationManager current_conversation;
	[HideInInspector]
	public static SceneManager scene_manager;

	public AudioSource current_music;	// Current looping background music
	
	private string conversation_log;	// Log of all previous conversations


	void Start () 
	{
		scene_manager = this;

		// Start the first conversation
		StartCoroutine(Start_Scene());
	}

	public IEnumerator Start_Scene()
	{
		yield return new WaitForSeconds(0.2f);
		starting_conversation.Start_Conversation();
	}


	// Pass in the game object that contains a 'ConversationManager' script to start
	public void Start_Conversation(GameObject conversation)
	{
		conversation.GetComponent<ConversationManager>().Start_Conversation();
	}


	// Fades out the current background music over seconds_it_takes_to_fade_out
	public IEnumerator Fade_Out_Music(float seconds_it_takes_to_fade_out)
	{
		if (current_music != null)
		{
			while (current_music.volume > 0)
			{
				current_music.volume -= 0.01f;
				yield return new WaitForSeconds(seconds_it_takes_to_fade_out / 100.0f);
			}
		}
	}


	// Sets the parent of music_object as the MusicManager, and then starts the AudioSource in music_object
	// music_object MUST have an attached audioSource component
	public void Set_Music(GameObject music_object)
	{
		GameObject music_manager = GameObject.Find("MusicManager");

		// Remove all previous background music
		foreach (Transform child in music_manager.transform) {
			GameObject.Destroy(child.gameObject);
		}

		// Replace with our new one
		music_object.transform.parent = music_manager.transform;
		music_object.GetComponent<AudioSource>().Play();
		current_music = music_object.GetComponent<AudioSource>();
	}


	public void Add_To_Log(string heading, string text)
	{
		conversation_log += heading + "\n";
		conversation_log += text + "\n";
	}


	void Update () 
	{
	
	}
}
