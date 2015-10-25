using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class TextFieldRestrict : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Replace() {
        string text = GetComponent<Text>().text;
        text = Regex.Replace(text, @"[^a-zA-Z0-9 ]", "");
    }

    
}
