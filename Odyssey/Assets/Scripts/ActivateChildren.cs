using UnityEngine;
using System.Collections;

// Activates all children upon Awake()
public class ActivateChildren : MonoBehaviour
{
    void Awake()
    {
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
