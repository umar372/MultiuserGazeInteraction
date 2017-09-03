using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour {


    private int player = 1;
    private Vector3 min, max;
    Vector3 mousepos,mousposP2;
    Transform cover,cover2;

    GameObject cursorGr, cursorRed;

    public bool isThisCardOpen;
    public string idName;
    // Use this for initialization
    void Start () {
  

        min = gameObject.GetComponent<Renderer>().bounds.min;
        max = gameObject.GetComponent<Renderer>().bounds.max;

        this.idName = this.GetComponent<SpriteRenderer>().sprite.name;
        cover = gameObject.transform.GetChild(1);
        cover2 = gameObject.transform.GetChild(2);

        cursorGr = GameObject.Find("cursorGreen");
        cursorRed = GameObject.Find("cursorRed");

    }

    // Update is called once per frame
    void Update () {

        mousepos = cursorGr.transform.position;
        mousposP2 = cursorRed.transform.position;
        if (min.x < mousepos.x && mousepos.x < max.x && min.y < mousepos.y && mousepos.y < max.y&&!isThisCardOpen)
        {
            cover.gameObject.SetActive(true);
            GameController.instance.highLightCardNameP1 = this.name;
        }else if (min.x < mousposP2.x && mousposP2.x < max.x && min.y < mousposP2.y && mousposP2.y < max.y&&!isThisCardOpen)
        {
            cover2.gameObject.SetActive(true);
            GameController.instance.highLightCardNameP2 = this.name;
        }
        else
        {
            cover.gameObject.SetActive(false);
            cover2.gameObject.SetActive(false);

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

