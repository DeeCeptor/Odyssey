using UnityEngine;
using System.Collections;

public class ScaleUp : MonoBehaviour
{
    public float rate = 1;


    void Update ()
    {
        Vector2 local_scale = this.transform.localScale;
        this.transform.localScale = new Vector3(local_scale.x + Time.deltaTime * rate,
            local_scale.x + Time.deltaTime * rate, 1);
	}
}
