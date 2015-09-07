using UnityEngine;
using System.Collections;


// Sets the looping background music that's currently playing
// This node must have a child object with an Audio Source so that the child
// can be moved to be a child of MusicManager
public class SetMusicNode : Node 
{
	public bool fadeOutPreviousMusic;
	public float fadeOutTime = 1.0f;

	public override void Run_Node()
	{
		if (fadeOutPreviousMusic && SceneManager.scene_manager.current_music != null)
		{
			// Fade out the previous background music for a smooth transition
			StartCoroutine(SceneManager.scene_manager.Fade_Out_Music(fadeOutTime));
			StartCoroutine(Wait(fadeOutTime));	// Wait, then add our background music
		}
		else
		{
			// If not fading out the previous background music, move the Child of this object that has the AudioSource
			// to be the child of MusicManager and have it play the AudioSource
			SceneManager.scene_manager.Set_Music(this.transform.GetChild(0).gameObject);
		}
	}


	// Waits a number of seconds before adding and playing the music. Allows the background music to fade out properly
	// before moving on.
	public IEnumerator Wait(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		SceneManager.scene_manager.Set_Music(this.transform.GetChild(0).gameObject);
		Finish_Node();
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
