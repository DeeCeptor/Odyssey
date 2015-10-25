using UnityEngine;
using System.Collections.Generic;

// Object that flies towards the targeted hex. Is destroyed once it reaches the hexes center and then shows the text given
public class AttackObject : MonoBehaviour
{
    public Hex hex_to_go_towards;
    public string string_to_display;
    public float speed = 3f;
    public Vector3 offset;
    public float text_duration;
    public int num_killed;
    public List<GameObject> dead_sprites = new List<GameObject>();

	void Start ()
    {
        
    }
	
	void Update ()
    {
        transform.position = Vector2.MoveTowards(transform.position, hex_to_go_towards.world_coordinates, Time.deltaTime * speed);

        float dist = Vector2.Distance(this.transform.position, hex_to_go_towards.world_coordinates);
        if (dist <= 0.2f)
        {
            ShowText();
        }
	}

    public void ShowText()
    {
        PlayerInterface.player_interface.CreateFloatingText(this.transform.position + offset, string_to_display, true, text_duration);
        //hex_to_go_towards.occupying_unit.AnimateCasualties(num_killed);
        AnimateDeaths();
        GameObject.Destroy(this.gameObject);
    }

    public void AnimateDeaths()
    {
        foreach(GameObject sprite in dead_sprites)
        {
            sprite.AddComponent<RotateSideways>();
        }
    }
}
