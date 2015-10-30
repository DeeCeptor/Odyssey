using UnityEngine;
using System.Collections;

public class StayAtPosition : MonoBehaviour 
{
	public Vector3 position_to_stay_at;

	void Start () {
	
	}
	
	void Update () {
		this.transform.position = position_to_stay_at;
	}
}
