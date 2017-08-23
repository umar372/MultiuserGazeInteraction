using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkDetection : MonoBehaviour
{
    public static BlinkDetection instance;
    public delegate void BlinkDelegate(double posX, double posY,string playerName);
    public event BlinkDelegate onBlinkHappen;

    double xPos = -1;
    double yPos = -1;
    public float confidence = -1;
    bool isGazeOnSurface = false;
    public PlayerData instanceP1;
    public bool isOpenedinco;
    bool isDetection;
    bool isCheckEyeOpen, isCoRoutineStarted;

    bool isUpdatePos;
    IEnumerator m_corot;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Use this for initialization
    void Start () {
        isCoRoutineStarted = false;
        isOpenedinco = true;
        isUpdatePos = true;
      
	}
	
	// Update is called once per frame
	void Update () {
        updateGazeValues();
        if (confidence > 0.9f && isGazeOnSurface)
        {
            isDetection = false;
        } else {
            isDetection = true;
        }
       
        DetectBlink();
    }


    void updateGazeValues()
    {
        if (instanceP1 == null)
        {
            Debug.Log("Didn't find gazeListener instance");
            return;
        } else {
            xPos = instanceP1.xpos;
            yPos = instanceP1.ypos;
            confidence = instanceP1.confidence;
            isGazeOnSurface = instanceP1.isOnSurface;

        }
    }

    void DetectBlink()
    {
        if (isDetection)
        {
            if (confidence < 0.9f)
            {
                if (!isCoRoutineStarted)
                {
                    StartCoroutine(waitOnZeroConf(0.35f));
                    isOpenedinco = false;
                    isCoRoutineStarted = true;
                    isUpdatePos = false;
                    //Debug.Log("Coroutiene Started");

                }

            }
        }

        if (isCoRoutineStarted && !isDetection)
        {
            isOpenedinco = true;
            isUpdatePos = true;
        }
        if (isDetection == false)
        {

            if (isCheckEyeOpen)
            {
                isCheckEyeOpen = false;
                Debug.Log("Blink Gesture Detected");
                isUpdatePos = true;
                if (onBlinkHappen != null)
                {
                    onBlinkHappen.Invoke(xPos, yPos,instanceP1.playerName);
                }
            }
            else { 
               

               // Debug.Log("Stopping Coroutine");
               // StopCoroutine("waitOnZeroConf");
            }
        }
    }

    IEnumerator waitOnZeroConf(float time)
    {
        //Debug.Log("In Enumerator");
        yield return new WaitForSeconds(time);
        //Debug.Log("Coroutene stopped");
        if(!isOpenedinco)
            isCheckEyeOpen = true;
        isCoRoutineStarted = false;

    }
}
