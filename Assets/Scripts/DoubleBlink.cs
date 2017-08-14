﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoubleBlink : MonoBehaviour
{
    double xPos = -1;
    double yPos = -1;
    float confidence = -1;
    float timeStamp = 0;
    bool isGazeOnSurface = false;
    public GazeListner instanceP1;

    bool isDetection, isOpenedinco;
    bool isCheckEyeOpen, isCoRoutineStarted;
    int BlinkCount;
    bool isTotalWait;
    float dTimer;

    IEnumerator m_corot;
    void Awake()
    {
        //instanceP1 = GazeListner.instance;
    }
    // Use this for initialization
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
            }
            
        }
        /*if (BlinkCount == 1 && !isTotalWait)
        {
            StartCoroutine(waitforDBlink(0.25f));
            isTotalWait = true;
        }*/
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
    IEnumerator waitforDBlink(float time)
    {
        //Debug.Log("In Enumerator");
        Debug.Log("Wait for second blink...");
        yield return new WaitForSeconds(time);
        //Debug.Log("Coroutene stopped");
        if (BlinkCount == 2)
        {
            BlinkCount = 0;
            isTotalWait = false;
            Debug.Log("Double Blink Detected");
        }

        else
        {
            BlinkCount = 0;
            isTotalWait = false;
            Debug.Log("No Double Blink Detected");
        }
    }
}