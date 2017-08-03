using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureDetection : MonoBehaviour {

    double xPos=-1;
    double yPos=-1;
    float confidence=-1;
    bool isGazeOnSurface = false;
    GazeListner instanceP1;
    void Awake()
    {
        instanceP1 = GazeListner.instance;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        updateGazeValues();
	}


    void updateGazeValues()
    {
        if (instanceP1 == null)
        {
            Debug.Log("Didn't find gazeListener instance");
        } else {
            xPos = instanceP1.xpos;
            yPos = instanceP1.ypos;
            confidence = instanceP1.confidence;
            isGazeOnSurface = instanceP1.isOnSurface;
        }
    }
}
