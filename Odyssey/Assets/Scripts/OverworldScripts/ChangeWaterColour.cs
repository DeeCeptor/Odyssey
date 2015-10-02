using UnityEngine;
using System.Collections;

public class ChangeWaterColour : MonoBehaviour
{
    public Color colorStart = Color.red;
    public Color colorEnd = Color.green;
    public float duration = 1.0F;
    public Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        /*
        Material mat = rend.material;
        mat.
        Debug.Log("A");
        foreach (string str in rend.material.shaderKeywords)
        {
            Debug.Log(str);
        }*/
    }

    void Update()
    {
        //float lerp = Mathf.PingPong(Time.time, duration) / duration;
        //rend.material.color = Color.Lerp(colorStart, colorEnd, lerp);


    }
}
