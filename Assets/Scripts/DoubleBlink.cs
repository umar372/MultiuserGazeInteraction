﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fixation : MonoBehaviour {

    double xPos = -1;
    double yPos = -1;
    float confidence = -1;
    bool isGazeOnSurface = false;
    public GazeListner glPlayer1;





    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
   
	}

    //To call this use StartCoroutine(FunctionForWait());
    IEnumerator FunctionForWait()
    {
    
    }
}
