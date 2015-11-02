using UnityEngine;
using System.Collections;

// Moves a UI element over the scene to mask our loading. Check finished variable to see if it's in position to change scenes
public class MoveUI : MonoBehaviour
{
    public static MoveUI transition_UI;

    RectTransform rect;
    public Vector3 movement_direction;
    Vector3 start_pos;
    Vector3 left_pos = new Vector3(-1500, 0, 0);

    public bool start_transitioning_out, start_transitioning_in;
    bool transition_out;
    bool transition_in;
    float cur_time;
    float max_time = 2;
    public bool finished = false;


    void Awake ()
    {
		if (MoveUI.transition_UI == null)
        	transition_UI = this;
		else 
		{
			// There's another moveUI, destroy this one
			Destroy(this.transform.parent.gameObject);
		}
    }


    void Start () {
        rect = this.GetComponent<RectTransform>();
        start_pos = rect.localPosition;

        if (start_transitioning_in)
            TransitionIn();
        else if (start_transitioning_out)
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
            {
                transition_in = false;
                finished = true;
                this.gameObject.SetActive(false);
            }
        }
        // Move from the far left to the center of the screen
        else if (transition_out)
        {
            rect.localPosition = Vector3.Lerp(left_pos, start_pos, Time.time - cur_time);

            if (rect.localPosition == start_pos)
            {
                transition_out = false;
                finished = true;
                this.gameObject.SetActive(false);
            }
        }
	}


    // Start out of sight on the left, and move until at center
    public void TransitionOut()
    {
        this.gameObject.SetActive(true);
        rect.localPosition = left_pos;
        transition_out = true;
        cur_time = Time.time;
        finished = false;
    }

    // Start at the center, and move to the right
    public void TransitionIn()
    {
        this.gameObject.SetActive(true);
        transition_in = true;
        cur_time = 0;
        rect.localPosition = start_pos;
        finished = false;
    }
}
