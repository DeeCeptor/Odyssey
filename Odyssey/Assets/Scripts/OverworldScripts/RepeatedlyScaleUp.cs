using UnityEngine;
using System.Collections;

public class RepeatedlyScaleUp : MonoBehaviour
{
    private Vector3 initial_scale;
    float expand_speed = 0.5f;
    float fade_speed = 0.5f;
    SpriteRenderer sprite;

    void Start()
    {
        initial_scale = this.transform.localScale;
        sprite = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Constantly increase scale to a factor
        Vector3 cur_scale = this.transform.localScale;
        cur_scale.x += Time.deltaTime * expand_speed;
        cur_scale.y += Time.deltaTime * expand_speed;
        this.transform.localScale = cur_scale;

        // Reset if transparent
        if (sprite.color.a <= 0)
        {
            sprite.color = Color.white;
            this.transform.localScale = initial_scale;
        }
        // Become transparent
        else if (cur_scale.x > initial_scale.x * 2)
        {
            Color c = sprite.color;
            c.a -= Time.deltaTime * fade_speed;
            sprite.color = c;
        }
    }
}
