using UnityEngine;
using System.Collections;

public class FollowTransform : MonoBehaviour 
{
    public Transform target;

	void Start () 
	{
	
	}
	
	void Update () 
	{
        Vector3 wantedPos = Camera.main.WorldToViewportPoint(target.position);
        this.transform.position = wantedPos;
	}
}
