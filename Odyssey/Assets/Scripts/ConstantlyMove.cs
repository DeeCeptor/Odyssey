using UnityEngine;
using System.Collections;

public class ConstantlyMove : MonoBehaviour
{
    public Vector2 direction;
    public float rate;

    void Update()
    {
        this.transform.position = this.transform.position + (Vector3)direction * rate * Time.deltaTime;
    }
}
