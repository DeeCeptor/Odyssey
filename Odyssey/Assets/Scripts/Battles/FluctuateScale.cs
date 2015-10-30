using UnityEngine;
using System.Collections;

public class FluctuateScale : MonoBehaviour
{
    private bool shrinking = true;
    float lower_range = 0.7f;
    float scale_rate = 0.5f;

	void Start () {
	
	}

    void OnEnable()
    {
        this.transform.localScale = Vector3.one;
    }

    void Update ()
    {
        Vector2 cur_scale = this.transform.localScale;
	    if (shrinking)
        {
            this.transform.localScale = new Vector3(cur_scale.x - Time.deltaTime * scale_rate,
                cur_scale.x - Time.deltaTime * scale_rate, 1);

            if (this.transform.localScale.x < lower_range)
                shrinking = false;
        }
        else
        {
            this.transform.localScale = new Vector3(cur_scale.x + Time.deltaTime * scale_rate,
                cur_scale.x + Time.deltaTime * scale_rate, 1);

            if (this.transform.localScale.x >= 1)
                shrinking = true;
        }
  	}
}
