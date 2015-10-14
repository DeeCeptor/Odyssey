using UnityEngine;
using System.Collections;

public class MoveUI : MonoBehaviour {
    RectTransform rect;
    public Vector3 movement_direction;

	// Use this for initialization
	void Start () {
        rect = this.GetComponent<RectTransform>();
        Debug.Log("B");
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("A");
        rect.localPosition = rect.localPosition + movement_direction;
	}
}
