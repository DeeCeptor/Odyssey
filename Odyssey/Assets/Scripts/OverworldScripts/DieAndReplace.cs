using UnityEngine;
using System.Collections;

public class DieAndReplace : MonoBehaviour {

    private float counterToDie = 0;
    public int LifeSpan = 240;
    public GameObject ReplaceWith;
    public bool paused = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!paused)
        {
            if (counterToDie > LifeSpan)// && (!this.GetComponent<Renderer>().isVisible))
            {
                Instantiate(ReplaceWith, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            counterToDie = counterToDie + 1;
        }
    }

    public void Pause()
    {
        paused = true;
    }

    public void Unpause()
    {
        paused = false;
    }
}
