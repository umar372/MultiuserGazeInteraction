using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NavigationGestures : MonoBehaviour {

    double gazeX = -1;
    double gazeY = -1;
    Vector3 gazeNormalized;
    public GazeListner glPlayer1;
    public bool magnifyLensActivated; //refers to the double blink
    bool blink; //refers to the blink
    bool selectionMode;
    bool isWaitForFixation;
    public GameObject lensFrame;
    float xPosLens = -1;
    float yPosLens = -1;
    Vector4 leftArea, rightArea, topArea, bottomArea;
    Vector4 endFix;
    float timer;


    // Use this for initialization
    void Start () {
        lensFrame = GameObject.Find("player1border");
        gazeNormalized = new Vector3(0f,0f,0f);
        isWaitForFixation = false;
        selectionMode = false;
        magnifyLensActivated = false;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(glPlayer1.xpos +" " + glPlayer1.ypos);

        if (magnifyLensActivated)
        {
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

            if(selectionMode)
            {
                DetectFixationPattern();
                if(isWaitForFixation)
                {
                    Debug.Log("gg");
                    WaitForFixationComplete(endFix);
                }
            }
            else
            {
                LockLens();
            }
        }
        else
        {
            selectionMode = false;
        }

	}

    void DetectFixationPattern(){

        if(!isWaitForFixation)
        {
            Debug.Log("Gaze Position xPos => " + gazeNormalized.x+ "  yPos => " + gazeNormalized.y);

            leftArea = new Vector4(xPosLens - 3.3f, xPosLens - 2.8f, yPosLens - 3.3f, yPosLens + 3.3f);
            rightArea = new Vector4(xPosLens + 2.8f, xPosLens + 3.3f, yPosLens - 3.3f, yPosLens + 3.3f);
            topArea = new Vector4(xPosLens - 3.3f, xPosLens + 3.3f, yPosLens + 2.8f, yPosLens + 3.3f);
            bottomArea = new Vector4 (xPosLens - 3.3f, xPosLens + 3.3f, yPosLens - 2.8f, yPosLens - 3.3f);
            //Debug.Log(isWaitForFixation);

            if(detectBorder(leftArea))
            {
                Debug.Log("Comees Left");
                isWaitForFixation = true;
                endFix = rightArea;
                timer = 0f;
            }
            if(detectBorder(rightArea))
            {
                isWaitForFixation = true;
                Debug.Log("Comees right");
                endFix = leftArea;
                timer = 0f;
            }
            if(detectBorder(topArea))
            {
                isWaitForFixation = true;
                Debug.Log("Comees top");
                endFix = bottomArea;
                timer = 0f;
            }
            if(detectBorder(bottomArea))
            {
                isWaitForFixation = true;
                Debug.Log("Comees bottom");
                endFix = topArea;
                timer = 0f;
            }
        }

    }

    bool WaitForFixationComplete(Vector4 endingFix)
    {   
        timer += Time.deltaTime;
        if (timer < 0.8f)
        {
            Debug.Log("IN WAITINGGGG " + timer);
            
            if (detectBorder(endingFix))
            {
                Debug.Log("Detected Fixation !!!!!!!!!!!!!!!!!!!!!! ");
                timer = 0f;
                isWaitForFixation = false;
                selectionMode = false;
                return true;

            }
        }
        else
        {

            timer = 0f;
            isWaitForFixation = false;
            return false;
        }
        return false;

    }

    bool detectBorder(Vector4 border)
    {
        //Debug.Log(border.x + " <= " + gazeNormalized.x + " and " + border.y + " >= " + gazeNormalized.x);
        if(border.x <= gazeNormalized.x && border.y >= gazeNormalized.x && 
            border.z <= gazeNormalized.y && border.w >= gazeNormalized.y )
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
        //Debug.Log("World Position " + worldPos);
        return worldPos;
    }


}
