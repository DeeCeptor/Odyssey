using UnityEngine;
using System.Collections;

public class MoveUI : MonoBehaviour {
    RectTransform rect;
    public Vector3 movement_direction;
    Vector3 start_pos;
    Vector3 left_pos = new Vector3(-1500, 0, 0);

    bool transition_out;
    bool transition_in;
    float cur_time;
    float max_time = 2;
    public bool finished = false;


    void Start () {
        rect = this.GetComponent<RectTransform>();
        start_pos = rect.localPosition;
        TransitionOut();
    }
	

	void Update ()
    {
        // Move from the center to the far right
        if (transition_in && cur_time < max_time)
        {
            cur_time += Time.deltaTime;
            rect.localPosition = rect.localPosition + movement_direction;

            if (cur_time >= max_time)
                finished = true;
        }
        // Move from the far left to the center of the screen
        else if (transition_out)
        {
            rect.localPosition = Vector3.Lerp(left_pos, start_pos, Time.time - cur_time);

            if (rect.localPosition == left_pos)
                finished = true;
        }
	}


    // Start out of sight on the left, and move until at center
    public void TransitionOut()
    {
        rect.localPosition = left_pos;
        transition_out = true;
        cur_time = Time.time; ;
        finished = false;
    }

    // Start at the center, and move to the right
    public void TransitionIn()
    {
        transition_in = true;
        cur_time = 0;
        rect.localPosition = start_pos;
        finished = false;
    }
}
