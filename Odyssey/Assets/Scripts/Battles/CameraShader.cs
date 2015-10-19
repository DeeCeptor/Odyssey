using UnityEngine;
using System.Collections;

public class CameraShader : MonoBehaviour
{

    public Shader shader;

	// Use this for initialization
	void Start () {
        Camera.main.SetReplacementShader(shader, "");//sets a global shader in that camera
        //Camera.main.ResetReplacementShader(); // resets back to the original
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
