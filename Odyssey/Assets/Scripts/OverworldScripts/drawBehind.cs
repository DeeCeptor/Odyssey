using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class drawBehind : MonoBehaviour {
    public LineRenderer lineRender;
    public float minDistance = 0.1f;
    private int numberOfPoints = 0;
    private LayerMask mask;
    public int LayerToNotDrawThrough = 11;
    private bool hit = false;
    public PlayerBoatController player;
    public Queue<Vector3> vertices;
    public GameObject X;
    public GameObject lastX;
    // Use this for initialization
    void Start () {
	mask = 1 << LayerToNotDrawThrough;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBoatController>();
        vertices = new Queue<Vector3>();
    }
	
	// Update is called once per frame
	void Update () {
        if (EventManagement.gameController.currentEvent==null)
        {
            BackOnMap();
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Destroy(lastX);
                transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.position.z);
                GetComponent<SpriteRenderer>().enabled = true;
                numberOfPoints = 0;
                lineRender.SetVertexCount(0);
                EventManagement.gameController.Pause();
                hit = false;
                vertices.Clear();
            }
            else if (Physics2D.OverlapCircle(transform.position, minDistance, mask))
            {
                if (GetComponent<SpriteRenderer>().enabled ==true)
                {
                   lastX = (GameObject)Instantiate(X, transform.position, transform.rotation);
                    lastX.transform.parent = GameObject.Find("UniversalParent").transform;
                }
                transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.position.z);
                GetComponent<SpriteRenderer>().enabled = false;
                hit = true;
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                if (!hit)
                {
                    numberOfPoints++;
                    lineRender.SetVertexCount(numberOfPoints);
                    lineRender.SetPosition(numberOfPoints - 1, transform.position);
                    vertices.Enqueue(transform.position);
                }

            }
            else
            {
                if (EventManagement.gameController.paused)
                {
                    if (GetComponent<SpriteRenderer>().enabled == true)
                    {
                        lastX = (GameObject)Instantiate(X, transform.position, transform.rotation);
                        lastX.transform.parent = GameObject.Find("UniversalParent").transform;
                    }
                    vertices.Dequeue();
                    EventManagement.gameController.Unpause();
                    player.vertices = vertices;
                    player.vertexIndex = 1;
                    GetComponent<SpriteRenderer>().enabled = false;
                    
                    Debug.Log(vertices.Count.ToString());
                }

            }
        }
    }

    public void BackOnMap()
    {

    }
}
