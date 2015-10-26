using UnityEngine;
using System.Collections;

public class FadeTextMesh : MonoBehaviour
{
    private float alpha = 1;
    private float fadingOverTime = 3.0f;
    private Color curColor;

    void Start()
    {
        curColor = transform.GetComponent<TextMesh>().color;
        transform.GetComponent<MeshRenderer>().sortingOrder = 4;
    }

    void Update()
    {
        alpha -= Mathf.Clamp01(Time.deltaTime / fadingOverTime);
        curColor.a = alpha;
        transform.GetComponent<TextMesh>().color = curColor;
    }
}
