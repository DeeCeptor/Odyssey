using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour
{
    public float fade_after = 0.5f;    // In seconds
    public float fade_rate = 1;

    private float cur_time;
    private SpriteRenderer sprite;

	void Start ()
    {
        sprite = this.GetComponent<SpriteRenderer>();
	}
	
	void Update ()
    {
        cur_time += Time.deltaTime;
        if (cur_time >= fade_after)
        {
            // Fade out
            Color cur_color = sprite.color;
            float alpha = cur_color.a;
            cur_color.a = cur_color.a - Mathf.Clamp01(Time.deltaTime / fade_rate);
            sprite.color = cur_color;
        }
	}
}
