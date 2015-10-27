using UnityEngine;
using System.Collections;

public class RepeatedlyScaleUp : MonoBehaviour
{
    private Vector3 initial_scale;
    float expand_speed = 0.2f;
    float fade_speed = 0.5f;
    float start_transparency_at_scale = 1.5f;
    SpriteRenderer sprite;
    SpriteRenderer[] children;
    void Start()
    {
        initial_scale = this.transform.localScale;
        sprite = this.GetComponent<SpriteRenderer>();
        float s_z = 0.1f;
        // Spawn additional smaller ripples
        for (float x = 0.9f; x > 0.5;)
        {
            GameObject obj = new GameObject("Ripples");
            obj.transform.position = this.transform.position;
            obj.transform.parent = this.transform;
            obj.transform.Translate(0, 0, s_z);
            obj.transform.localScale = new Vector3(x, x, 1);
            obj.AddComponent<SpriteRenderer>();
            obj.GetComponent<SpriteRenderer>().sprite = this.GetComponent<SpriteRenderer>().sprite;
            x -= 0.1f;
            s_z += 0.1f;
        }

        children = this.GetComponentsInChildren<SpriteRenderer>();
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
            foreach (SpriteRenderer s in children)
            {
                s.color = Color.white;
            }
            this.transform.localScale = initial_scale;
        }
        // Become transparent
        else if (cur_scale.x > initial_scale.x * start_transparency_at_scale)
        {
            Color c = sprite.color;
            c.a -= Time.deltaTime * fade_speed;
            sprite.color = c;

            foreach (SpriteRenderer s in children)
            {
                s.color = c;
            }
        }
    }
}
