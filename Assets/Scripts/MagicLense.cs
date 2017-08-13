using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLense : MonoBehaviour {
    private Camera magnifyCamera;
    private float mgfcWidth = Screen.width/5f;
    private float mgfcHeight = Screen.height / 5f;

    float MGOX;
    float MG0Y;


    // Use this for initialization
    void Start () {
        createGlassForMagnifying();

    }
	
	// Update is called once per frame
	void Update () {
       // magnifyCamera.pixelRect = new Rect(Input.mousePosition.x - mgfcWidth / 2.0f, Input.mousePosition.y - mgfcHeight / 2.0f, mgfcWidth, mgfcHeight);

    }

    void createGlassForMagnifying()
    {
        GameObject camera = new GameObject("MagnifyCamera");
        MGOX = Screen.width / 2f - mgfcWidth / 2f;
        MG0Y = Screen.height / 2f - mgfcHeight / 2f;
        magnifyCamera = camera.AddComponent<Camera>();
        magnifyCamera.pixelRect = new Rect(MGOX, MG0Y, mgfcWidth, mgfcHeight);
        magnifyCamera.transform.position = new Vector3(0, 0, 0);
    }
}
