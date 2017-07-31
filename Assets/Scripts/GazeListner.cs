using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NetMQ; // for NetMQConfig
using NetMQ.Sockets;
using System.Threading;
using MsgPack;
using UnityEngine.UI;
using LitJson;


namespace GazeData {

   

    [Serializable]
    public class GazeData2D
    {

        public string topic = "";
        public double timestamp = 0.0;
        public double confidence = -1.0;
        public bool on_srf = false;
        public double[] norm_pos = new double[] { 0.0, 0.0 };

    }
}


public class GazeListner : MonoBehaviour {


    public delegate void DwellBlinkDelegate(double posX, double posY);
    public event DwellBlinkDelegate onBlinkHappen;

    private JsonData itemData;
    public Text normPosText,confidenceText,isOneSurface,blinkDetected;

    Thread client_thread_;
    private System.Object thisLock_ = new System.Object();
    bool stop_thread_ = false;
    public string IP;
    public string PORT;
    public string ID;

    bool isDetectGesture,startedDetection,boolAgainBlink;
    float timer;
    int numOfBlinks;

    double xpos = -1;
    double ypos = -1;
    float confidence= -1;
    bool isOnSurface;



    GazeData.GazeData2D data_ = new GazeData.GazeData2D();

    public void get_transform()
    {
        lock (thisLock_)
        {
            if (itemData != null)
            {

                if (itemData.Keys.Contains("gaze_on_srf"))
                {
                    if (itemData["gaze_on_srf"].Count > 0)
                    {

                        xpos = Math.Round((double)itemData["gaze_on_srf"][0]["norm_pos"][0], 2);
                        ypos = Math.Round((double)itemData["gaze_on_srf"][0]["norm_pos"][1], 2);
                        normPosText.text = "Norm " + xpos + " , " + ypos;

                    }
                    if (itemData["gaze_on_srf"].Count > 0)
                    {
                        string conf = itemData["gaze_on_srf"][0]["confidence"].ToString();
                        confidence = float.Parse(conf);
                        confidenceText.text = "conf " + confidence;
                        isOnSurface = (bool)itemData["gaze_on_srf"][0]["on_srf"];
                        isOneSurface.text = "IsOnSurf " + (bool)isOneSurface;


                    }
                }

               if (itemData.Keys.Contains("is_blink"))
                {
                    // blinkDetected.text = "" + itemData["is_blink"];
                    if ((bool)itemData["is_blink"] && boolAgainBlink)
                    {
                        numOfBlinks += 1;
                        boolAgainBlink = false;
                    }
                    else {
                        boolAgainBlink = true;
                    }
                   
                        


                }

                if (numOfBlinks > 0)
                {
                    timer += Time.deltaTime;
                    if (timer > 2f && numOfBlinks < 2)
                    {
                        timer = 0;
                        numOfBlinks = 0;
                    }
                   /* if (timer > 0.1f && numOfBlinks < 2)
                    {
                        boolAgainBlink = true;
                    }*/

                    if (numOfBlinks == 2)
                    {
                        timer = 0;
                        numOfBlinks = 0;
                        boolAgainBlink = true;
                        if (onBlinkHappen != null)
                        {
                            onBlinkHappen.Invoke(xpos,ypos);
                        }
                    }
                }
                
            }
            


        }
    }
    // Use this for initialization
    void Start () {

        boolAgainBlink = true;
        Debug.Log("Start a request thread.");
        client_thread_ = new Thread(NetMQClient);
        client_thread_.Start();
    }
	
	// Update is called once per frame
	void Update () {
        get_transform();

        if (confidence >=0.91f && isOnSurface)
        {
            isDetectGesture = true;
        }

       // BlinkDetect();

    }

    void BlinkDetect()
    {
        if (isDetectGesture == true)
        {
            if (confidence < 1f)
            {
                isDetectGesture = false;
                startedDetection = true;

            }
        }
        
        if (startedDetection)
        {
            timer = timer + Time.deltaTime;
            Debug.Log("In Timer "+timer);

            if (timer > 0.10f)
            {
                Debug.Log("More than 500");
                timer = 0;

                if (confidence >= 0.9f)
                {
                    Debug.Log("Blink detected");
                    blinkDetected.text = "yes";
                    startedDetection = false;
                    if (onBlinkHappen != null)
                        onBlinkHappen.Invoke(xpos,ypos);

                }
            }
            else if(confidence ==1f){
                Debug.Log("Out Timer");

                startedDetection = false;
                blinkDetected.text = "no";
                timer = 0f;

            }
        }
        
    }


    void NetMQClient()
    {
        string IPHeader = ">tcp://" + IP + ":";
        var timeout = new System.TimeSpan(0, 0, 1); //1sec

        // Necessary to handle this NetMQ issue on Unity editor
        // https://github.com/zeromq/netmq/issues/526
        AsyncIO.ForceDotNet.Force();
        NetMQConfig.ManualTerminationTakeOver();
        NetMQConfig.ContextCreate(true);

        string subport;
        Debug.Log("Connect to the server: " + IPHeader + PORT + ".");
        var requestSocket = new RequestSocket(IPHeader + PORT);
        requestSocket.SendFrame("SUB_PORT");
        bool is_connected = requestSocket.TryReceiveFrameString(timeout, out subport);
        requestSocket.Close();

        if (is_connected)
        {
            // 
            var subscriberSocket = new SubscriberSocket(IPHeader + subport);
            subscriberSocket.Subscribe(ID);
            subscriberSocket.Subscribe("blinks");

            var msg = new NetMQMessage();
            while (is_connected && stop_thread_ == false)
            {
               // Debug.Log("Receive a multipart message.");
                is_connected = subscriberSocket.TryReceiveMultipartMessage(timeout, ref (msg));
                if (is_connected)
                {
                    //Debug.Log("Unpack a received multipart message.");
                    try
                    {
                        //Debug.Log(msg[0].ConvertToString());
                        var message = MsgPack.Unpacking.UnpackObject(msg[1].ToByteArray());
                        MsgPack.MessagePackObject mmap = message.Value;
                        lock (thisLock_)
                        {
                            itemData = JsonMapper.ToObject(mmap.ToString());

                        }
                        ///Debug.Log(message);
                    }
                    catch
                    {
                        Debug.Log("Failed to unpack.");
                    }
                }
                else
                {
                    Debug.Log("Failed to receive a message.");
                    Thread.Sleep(1000);
                }
            }
            subscriberSocket.Close();
        }
        else
        {
            Debug.Log("Failed to connect the server.");
        }

        // Necessary to handle this NetMQ issue on Unity editor
        // https://github.com/zeromq/netmq/issues/526
        Debug.Log("ContextTerminate.");
        NetMQConfig.ContextTerminate();
    }


    void OnApplicationQuit()
    {
        lock (thisLock_) stop_thread_ = true;
        client_thread_.Join();
        Debug.Log("Quit the thread.");
    }
}
