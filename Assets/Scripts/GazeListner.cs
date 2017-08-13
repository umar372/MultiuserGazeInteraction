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

    public static GazeListner instance;
    public delegate void DwellBlinkDelegate(double posX, double posY);
    public event DwellBlinkDelegate onBlinkHappen;

    public JsonData itemData;
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

    public double xpos = -1;
    public double ypos = -1;
    public float confidence= -1;
    public bool isOnSurface;


    int failed_count_secs = 0;


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
            }
        }
    }

    // Use this for initialization
    void Start () {
        failed_count_secs = 0;
        Debug.Log("Start a request thread.");
        client_thread_ = new Thread(NetMQClient);
        client_thread_.Start();
    }
	
	// Update is called once per frame
	void Update () {
        get_transform();

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
            subscriberSocket.Subscribe("fixations");

            var msg = new NetMQMessage();
            while ((is_connected || failed_count_secs <20) && stop_thread_ == false)
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
                        Debug.Log(message);
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
                    failed_count_secs += 1;
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
