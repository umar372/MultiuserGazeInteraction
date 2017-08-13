using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NavigationGestures : MonoBehaviour {

    double gazeX = -1;
    double gazeY = -1;
    public GazeListner glPlayer1;
    public bool magnifyLensActivated =false;
    bool selectionMode = false;
    public GameObject lensFrame;
    float xPosLens = -1;
    float yPosLens = -1;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        xPosLens = lensFrame.transform.position.x;
        yPosLens = lensFrame.transform.position.y;
        gazeX = glPlayer1.x * Screen.width;
        gazeY = glPlayer1.y * Screen.height;

        if(magnifyLensActivated)
        {
            if(selectionMode)
            {
                DetectFixationPattern();
            }
            else
            {
                LockLens();
            }
        }
	}

    void DetectFixationPattern(){
        leftArea = [xPosLens - 1.2, xPosLens - 0.8, yPosLens - 1.2, yPosLens + 1.2];
        rightArea = [xPosLens + 0.8, xPosLens - 1.2, yPosLens - 1.2, yPosLens + 1.2];
        topArea = [xPosLens - 1.2, xPosLens + 1.2, yPosLens + 0.8, yPosLens + 1.2];
        bottomArea = [xPosLens - 1.2, xPosLens + 1.2, yPosLens - 0.8, yPosLens - 1.2];

        if(detectBorder(leftArea))
        {
            if(detectBorder(rightArea))
            {
                Debug.Log("left -> right";)
            }
        }
        if(detectBorder(rightArea))
        {
            if(detectBorder(leftArea))
            {
                Debug.Log("right -> left";)
            }
        }
        if(detectBorder(topArea))
        {
            if(detectBorder(bottomArea))
            {
                Debug.Log("top -> bottom";)
            }
        }
        if(detectBorder(bottomArea))
        {
            if(detectBorder(topArea))
            {
                Debug.Log("bottom -> top";)
            }
        }


    }

    bool detectBorder(float[] border)
    {
        if(border[0] >= gazeX && border[1] <= gazeX && border[2] >= gazeY && border[3] <= gazeY )
        {
            return true;
        }
        return false;
    }

    void LockLens()
    {
        if(blink)
        {
            selectionMode = true;
        }
    }

    void 
}
