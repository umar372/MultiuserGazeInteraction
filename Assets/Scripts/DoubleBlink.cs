using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoubleBlink : MonoBehaviour
{
    public delegate void DoubleBlinkDelegate(double posX, double posY,string playerName);
    public event DoubleBlinkDelegate onDoubleBlinkHappen;
    double xPos = -1;
    double yPos = -1;
    float confidence = -1;
    float timeStamp = 0;
    bool isGazeOnSurface = false;
    public PlayerData instanceP1;

    bool isDetection, isOpenedinco;
    bool isCheckEyeOpen, isCoRoutineStarted;
    int BlinkCount;
    bool isTotalWait;
    float dTimer;

    IEnumerator m_corot;
   
    void Start()
    {
        isCoRoutineStarted = false;
        isOpenedinco = false;
        isTotalWait = false;
        BlinkCount = 0;
        dTimer = 0f;
        Debug.Log("Starting...");
    }

    // Update is called once per frame
    void Update()
    {
        updateGazeValues();
        if (confidence > 0.9f && isGazeOnSurface)
        {
            isDetection = false;
        }
        else
        {
            isDetection = true;
        }
        DetectBlink();
        if(BlinkCount >= 1)
        {
            dTimer += Time.deltaTime;
            if(dTimer > 1.0f && BlinkCount != 2)
            {
                dTimer = 0f;
                BlinkCount = 0;
                Debug.Log("Gesture not detected");
            }
            else if(BlinkCount == 2)
            {
                dTimer = 0f;
                BlinkCount = 0;
                Debug.Log("Gesture detected");
                if (onDoubleBlinkHappen != null)
                {
                    onDoubleBlinkHappen.Invoke(xPos, yPos,instanceP1.playerName);
                }
            }
            
        }
    
    }


    void updateGazeValues()
    {
        if (instanceP1 == null)
        {
            Debug.Log("Didn't find gazeListener instance");
            return;
        }
        else
        {
            xPos = instanceP1.xpos;
            yPos = instanceP1.ypos;
            confidence = instanceP1.confidence;

            isGazeOnSurface = instanceP1.isOnSurface;
            //Debug.Log("confidence "+confidence);
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
                    StartCoroutine(waitOnZeroConf(0.2f));
                    isOpenedinco = false;
                    isCoRoutineStarted = true;
                    //Debug.Log("Coroutiene Started");

                }

            }
        }

        if (isCoRoutineStarted && !isDetection)
        {
            isOpenedinco = true;
        }
        if (isDetection == false)
        {

            if (isCheckEyeOpen)
            {
                isCheckEyeOpen = false;
                BlinkCount += 1;
                Debug.Log("Blink Gesture Detected " +BlinkCount);
            }
            else
            {


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
        if (!isOpenedinco)
            isCheckEyeOpen = true;
        isCoRoutineStarted = false;

    }

}
