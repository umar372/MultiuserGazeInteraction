using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour {

    //public GazeListner gazeListner;
    //public GameObject[] cards;
    private int player = 1;
    private Vector3 min, max;
    Vector2 mousePos;// = new Vector2();
    PlayerData p1;
    //double xPos, yPos;
    // Use this for initialization
    void Start () {
        min = gameObject.GetComponent<Renderer>().bounds.min;
        max = gameObject.GetComponent<Renderer>().bounds.max;
        mousePos = new Vector2();
    }
	
	// Update is called once per frame
	void Update () {
        Transform cover = gameObject.transform.GetChild(player);
        // Vector3 mousepos = getWorldPosition(Input.mousePosition);
        mousePos.x = (float)p1.xpos;
        mousePos.y = (float)p1.ypos;
        if (true)//min.x < mousepos.x && mousepos.x < max.x && min.y < mousepos.y && mousepos.y < max.y)
        {
                cover.gameObject.SetActive(true);
        }
        else
        {
            cover.gameObject.SetActive(false);
        }
	}
    public Vector3 getWorldPosition(Vector3 screenPos)
    {
        Vector3 worldPos;
        if (Camera.main.orthographic)
        {
            worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = Camera.main.transform.position.z;
        }
        else
        {
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.transform.position.z));
            worldPos.x *= -1;
            worldPos.y *= -1;
        }
        return worldPos;
    }
}

