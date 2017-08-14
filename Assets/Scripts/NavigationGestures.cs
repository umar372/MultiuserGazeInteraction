using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NavigationGestures : MonoBehaviour {

    double gazeX = -1;
    double gazeY = -1;
    Vector3 gazeNormalized;
    public GazeListner glPlayer1;
    public bool magnifyLensActivated =false;
    bool selectionMode = false;
    public GameObject lensFrame;
    float xPosLens = -1;
    float yPosLens = -1;
    Vector4 leftArea, rightArea, topArea, bottomArea;

    bool isWaitForFixation;
    float timer;
   

    // Use this for initialization
    void Start () {
        lensFrame = GameObject.Find("player1border");
        gazeNormalized = new Vector3(0f,0f,0f);
        isWaitForFixation = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (lensFrame == null)
        {
            lensFrame = GameObject.Find("player1border");
        }
        else {
            xPosLens = lensFrame.transform.position.x;
            yPosLens = lensFrame.transform.position.y;
        }
      
		gazeX = glPlayer1.xpos * Screen.width;
		gazeY = glPlayer1.ypos * Screen.height;
        gazeNormalized = new Vector3((float)gazeX,(float)gazeY,0f);
		gazeNormalized = getWorldPosition(gazeNormalized);

        DetectFixationPattern();

        if (magnifyLensActivated)
        {
            if(selectionMode)
            {
            }
            else
            {
                LockLens();
            }
        }


	}

    void DetectFixationPattern(){
        Debug.Log("Lense Position xPos => " + xPosLens+ "  yPos => " + yPosLens);
        Debug.Log("Gaze Position xPos => " + gazeNormalized.x+ "  yPos => " + gazeNormalized.y);

        leftArea = new Vector4(xPosLens - 1.2f, xPosLens - 0.8f, yPosLens - 1.2f, yPosLens + 1.2f);
        rightArea = new Vector4(xPosLens + 0.8f, xPosLens - 1.2f, yPosLens - 1.2f, yPosLens + 1.2f);
        topArea = new Vector4(xPosLens - 1.2f, xPosLens + 1.2f, yPosLens + 0.8f, yPosLens + 1.2f);
        bottomArea = new Vector4 (xPosLens - 1.2f, xPosLens + 1.2f, yPosLens - 0.8f, yPosLens - 1.2f);

        if(detectBorder(leftArea))
        {
            Debug.Log("Comees Left");
            isWaitForFixation = true;
            if(WaitForFiationComplete(rightArea))
            {
                Debug.Log("left -> right");
            }
        }
        if(detectBorder(rightArea))
        {
            isWaitForFixation = true;
            Debug.Log("Comees right");

            if (WaitForFiationComplete(leftArea))
            {
                Debug.Log("right -> left");
            }
        }
        if(detectBorder(topArea))
        {
            isWaitForFixation = true;

            if (WaitForFiationComplete(bottomArea))
            {
                Debug.Log("top -> bottom");
            }
        }
        if(detectBorder(bottomArea))
        {
            isWaitForFixation = true;
            if (WaitForFiationComplete(topArea))
            {
                Debug.Log("bottom -> top");
            }
        }


    }

    bool WaitForFiationComplete(Vector4 endingFix)
    {
        if (isWaitForFixation)
        {
            
            timer += Time.deltaTime;
            if (detectBorder(endingFix) && timer < 0.8f)
            {
                Debug.Log("Detected Fixation on ");
                timer = 0f;
                isWaitForFixation = false;
                return true;

            }
            if (timer > 0.8f)
            {

                timer = 0f;
                isWaitForFixation = false;
                return false;
            }
        }
        else {
            timer = 0f;
        }

        return false;

    }

    bool detectBorder(Vector4 border)
    {
        if(border.x >= gazeNormalized.x && border.y <= gazeNormalized.x && 
            border.z >= gazeNormalized.y && border.w <= gazeNormalized.y )
        {
            return true;
        }
        return false;
    }

    void LockLens()
    {
       // if(blink)
       // {
         //   selectionMode = true;
       // }
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
        Debug.Log("World Position " + worldPos);
        return worldPos;
    }


}
