using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Actor : MonoBehaviour
{
	public string name;		// Actor name, use this name to reference this actor in dialogue
	public ActorManager.Actor_Positions position;
	private float brigthness_change = 0.05f;

	// Makes this actor play the designated animation from its animator controller
	public void Play_Animation(string animation_name)
	{

	}


	// Calls a coroutine to fade in this actor over a number of seconds specified by over_time
	public void Fade_In(float over_time)
	{
		StartCoroutine(Fade_In_Coroutine(over_time));
	}
	IEnumerator Fade_In_Coroutine(float over_time)
	{
		float value = 0;
		// Set it to completely transparent
		this.GetComponent<Image>().color = new Color(
			this.GetComponent<Image>().color.r,
			this.GetComponent<Image>().color.g,
			this.GetComponent<Image>().color.b,
			0);

		// Incrementally make it less transparent
		while (this.GetComponent<Image>().color.a != 1)
		{
			value += over_time / 100f;
			this.GetComponent<Image>().color = new Color(
				this.GetComponent<Image>().color.r,
				this.GetComponent<Image>().color.g,
				this.GetComponent<Image>().color.b,
				Mathf.Lerp(0, 1, value));
			yield return new WaitForSeconds(over_time / 100);
		}
		yield break;
	}


	// Calls a coroutine to slide in this actor over a number of seconds specified by over_time
	public void Slide_In(float over_time)
	{
		StartCoroutine(Fade_In_Coroutine(over_time));
	}
	IEnumerator Slide_In_Coroutine(float over_time)
	{
		yield break;
	}


	public void Darken()
	{
		StopAllCoroutines();
		StartCoroutine(Darken_Coroutine());
	}
	IEnumerator Darken_Coroutine()
	{
		float value = 0;
		while (this.GetComponent<Image>().color != Color.gray)
		{
			value += brigthness_change;
			this.GetComponent<Image>().color = Color.Lerp(Color.white, Color.black, value);
			yield return new WaitForSeconds(0.03f);
		}
		yield break;
	}


	public void Lighten()
	{
		StopAllCoroutines();
		StartCoroutine(Lighten_Coroutine());
	}
	IEnumerator Lighten_Coroutine()
	{
		float value = 0;
		while (this.GetComponent<Image>().color != Color.white)
		{
			value += brigthness_change;
			this.GetComponent<Image>().color = Color.Lerp(Color.gray, Color.white, value);
			yield return new WaitForSeconds(0.03f);
		}
		yield break;
	}


	// Instantly places the actor at the designated position
	public void Place_At_Position(ActorManager.Actor_Positions destination)
	{
		position = destination;

		RectTransform destination_pos = ActorManager.Get_Position(destination);
		RectTransform cur_pos = this.GetComponent<RectTransform>();
		//cur_pos.position = destination_pos.position;

		cur_pos.offsetMin = new Vector2(destination_pos.offsetMin.x, destination_pos.offsetMin.y);
		cur_pos.offsetMax = new Vector2(destination_pos.offsetMax.x, destination_pos.offsetMin.y);
	}
}
