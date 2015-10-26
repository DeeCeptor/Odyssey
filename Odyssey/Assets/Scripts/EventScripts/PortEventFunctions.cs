using UnityEngine;
using System.Collections;

public class PortEventFunctions : IslandEventFunctions{
    public GameObject PortMenu;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Port()
    {
        Instantiate(PortMenu);
        Destroy(gameObject);
    }
}
