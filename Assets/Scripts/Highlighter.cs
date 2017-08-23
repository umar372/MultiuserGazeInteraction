using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour {

    //public GazeListner gazeListner;
    //public GameObject[] cards;
    private int player = 1;
    private Vector3 min, max;
    Vector3 mousepos;
    Transform cover;

    Vector2 mousePos;// = new Vector2();
    public PlayerData p1;

    public BlinkDetection blDet;

    GameObject cursorGr, cursorRed;
    //double xPos, yPos;
    // Use this for initialization
    void Start () {
        blDet = BlinkDetection.instance;
        min = gameObject.GetComponent<Renderer>().bounds.min;
        max = gameObject.GetComponent<Renderer>().bounds.max;
        cover = gameObject.transform.GetChild(player);

        cursorGr = GameObject.Find("cursorGreen");
    }

    // Update is called once per frame
    void Update () {
        if (cursorGr == null)
        {
            mousepos = getWorldPosition(Input.mousePosition);
        }
        else {
            Debug.Log("Found green and giving pos");
            mousepos = cursorGr.transform.position;
        }
        if (min.x < mousepos.x && mousepos.x < max.x && min.y < mousepos.y && mousepos.y < max.y)
        {
                cover.gameObject.SetActive(true);
            GenerateBoard.instance.highLightCardName = this.name;
        }
        else
        {
            cover.gameObject.SetActive(false);
        }

        /*blDet.onBlinkHappen += (double x, double y, string name) =>
        {
            if (min.x < mousepos.x && mousepos.x < max.x && min.y < mousepos.y && mousepos.y < max.y)
            {
                foreach (Transform t in this.transform)
                {
                    t.gameObject.SetActive(false);
                    t.gameObject.SetActive(false);
                }
            }
        };*/
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

