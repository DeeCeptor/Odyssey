using UnityEngine;
using System.Collections;

public class SpriteAnimInstruct : MonoBehaviour {

	//DECLARE NEW ANIMATOR REFERENCE
	Animator animator;

	// Use this for initialization
	void Start () {

		//INITIALIZING ANIMATOR
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void MoveAnim()
	{
		animator.SetTrigger ("triggerMove");
	}

	public void AttackAnim()
	{
		animator.SetTrigger ("triggerAttack");
	}

	public void StopAnim()
	{
		animator.SetTrigger ("triggerStop");
	}
}
