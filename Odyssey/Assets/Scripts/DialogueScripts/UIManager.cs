using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager ui_manager;
	public Text dialogue_text_panel;
	public Text speaker_text_panel;
	public GameObject choice_panel;
	public Image background;	// Background image
	public Image foreground;

	// Use this for initialization
	void Start () {
		ui_manager = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
