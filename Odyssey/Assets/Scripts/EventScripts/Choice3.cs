using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections;


// Displays the choices outlined. Does not continue to the next node.
// Each choice leads to a prescribed conversation.
public class Choice3 : Node
{
    [TextArea(4, 10)]
    public string choice_text;  // A description of the choice to be made
    public string button_1_text;
    public Button.ButtonClickedEvent Button_1_Events;
    public string button_2_text;
    public Button.ButtonClickedEvent Button_2_Events;
    public string button_3_text;
    public Button.ButtonClickedEvent Button_3_Events;


    public override void Run_Node()
    {
        // Display the choices on the UI
        UIManager.ui_manager.choice_panel.SetActive(true);

        UIManager.ui_manager.choice_panel.transform.FindChild("Text").GetComponent<Text>().text = choice_text;

        // Make buttons that have events visible, set their text,
        // add call to Finis_Node() on the OnClick() listener and hook up the choices buttons to the events on this node
        if (Button_1_Events.GetPersistentEventCount() > 0)
        {
            UIManager.ui_manager.choice_panel.transform.FindChild("Choice1").gameObject.SetActive(true);    // Make visible
            UIManager.ui_manager.choice_panel.transform.FindChild("Choice1").GetComponent<Button>().GetComponentInChildren<Text>().text = button_1_text;    // Set button text
            Button_1_Events.AddListener(Clear_Choices); // Add call to finish this node and hide UI to event listener
            UIManager.ui_manager.choice_panel.transform.FindChild("Choice1").GetComponent<Button>().onClick = Button_1_Events;  // Set events
        }
        if (Button_2_Events.GetPersistentEventCount() > 0)
        {
            UIManager.ui_manager.choice_panel.transform.FindChild("Choice2").gameObject.SetActive(true);
            UIManager.ui_manager.choice_panel.transform.FindChild("Choice2").GetComponent<Button>().GetComponentInChildren<Text>().text = button_2_text;
            Button_2_Events.AddListener(Clear_Choices);
            UIManager.ui_manager.choice_panel.transform.FindChild("Choice2").GetComponent<Button>().onClick = Button_2_Events;
        }
        if (Button_3_Events.GetPersistentEventCount() > 0)
        {
            UIManager.ui_manager.choice_panel.transform.FindChild("Choice3").gameObject.SetActive(true);
            UIManager.ui_manager.choice_panel.transform.FindChild("Choice3").GetComponent<Button>().GetComponentInChildren<Text>().text = button_3_text;
            Button_3_Events.AddListener(Clear_Choices);
            UIManager.ui_manager.choice_panel.transform.FindChild("Choice3").GetComponent<Button>().onClick = Button_3_Events;
        }
    }


    public override void Button_Pressed()
    {

    }


    public void Clear_Choices()
    {
        // Remove event listeners from buttons
        UIManager.ui_manager.choice_panel.transform.FindChild("Choice1").GetComponent<Button>().onClick = null;
        UIManager.ui_manager.choice_panel.transform.FindChild("Choice2").GetComponent<Button>().onClick = null;
        UIManager.ui_manager.choice_panel.transform.FindChild("Choice3").GetComponent<Button>().onClick = null;

        // Set all choice buttons to inactive
        UIManager.ui_manager.choice_panel.transform.FindChild("Choice1").gameObject.SetActive(false);
        UIManager.ui_manager.choice_panel.transform.FindChild("Choice2").gameObject.SetActive(false);
        UIManager.ui_manager.choice_panel.transform.FindChild("Choice3").gameObject.SetActive(false);


        // Hide choice UI
        UIManager.ui_manager.choice_panel.SetActive(false);
    }


    public override void Finish_Node()
    {
        Clear_Choices();        // Hide the UI
        base.Finish_Node();     // Continue conversation
    }
}
