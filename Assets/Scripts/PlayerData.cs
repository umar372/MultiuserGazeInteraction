using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    public GazeListner gl;

    public double xpos;
    public double ypos;
    public float confidence;
    public bool isOnSurface;
    public string playerName;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gl == null && playerName != null)
        {
            Debug.LogError("No Gaze Listner or Player Name");
            return;
        }
        else if (playerName == "player1")
        {
            xpos = gl.xposP1;
            ypos = gl.yposP1;
            confidence = gl.confidence_1;
            isOnSurface = gl.isOnSurface_1;
          //  Debug.Log("player name "+playerName+" x and y "+xpos+" "+ypos+" conf "+confidence+" isOnSurf "+isOnSurface);

        }
        else if (playerName == "player2")
        {
            xpos = gl.xposP2;
            ypos = gl.yposP2;
            confidence = gl.confidence_2;
            isOnSurface = gl.isOnSurface_2;
            //Debug.Log("player name " + playerName + " x and y " + xpos + " " + ypos + " conf " + confidence + " isOnSurf " + isOnSurface);

        }
    }
}
