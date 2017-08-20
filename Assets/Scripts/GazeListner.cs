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
using System.Net.Sockets;
using System.Text;
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
   

    public JsonData itemData;
    public Text normPosText,confidenceText,isOneSurface,blinkDetected;

    Thread client_thread_P1;
    private System.Object thisLock_P1 = new System.Object();
    bool stop_thread_ = false;
    public string IP;
    public string PORT;
    public string ID;

    bool isDetectGesture,startedDetection,boolAgainBlink;
    float timer;
    int numOfBlinks;

    private double xPosInst=-1,yPosInst=-1,xPosSum=0,yPosSum=0;
    private int numOfSums;

    public double xpos = -1;
    public double ypos = -1;

    public float confidence= -1;
    public bool isOnSurface;


    int failed_count_secs = 0;


    GazeData.GazeData2D data_ = new GazeData.GazeData2D();

 
    public void get_transform()
    {
        lock (thisLock_P1)
        {
            if (itemData != null)
            {

                if (itemData.Keys.Contains("gaze_on_srf"))
                {
                    if (itemData["gaze_on_srf"].Count > 0)
                    {

                        xPosInst = Math.Round((double)itemData["gaze_on_srf"][0]["norm_pos"][0], 2);
                        yPosInst = Math.Round((double)itemData["gaze_on_srf"][0]["norm_pos"][1], 2);
                        normPosText.text = "Norm " + xPosInst + " , " + yPosInst;

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
        client_thread_P1 = new Thread(NetMQClientP1);
        client_thread_P1.Start();
    }
	
	// Update is called once per frame
	void Update () {
        get_transform();
        if (isOneSurface)
        {
            xPosSum += xPosInst;
            yPosSum += yPosInst;
            numOfSums += 1;
            if (numOfSums == 10)
            {
                xpos = xPosSum / numOfSums;
                ypos = yPosSum / numOfSums;
                xPosSum = 0;
                yPosSum = 0;
                numOfSums = 0;
            }


        }
    }

   


    void NetMQClientP1()
    {
        string IPHeader = ">tcp://" + IP + ":";
        var timeout_p1 = new System.TimeSpan(0, 0, 1); //1sec

        // Necessary to handle this NetMQ issue on Unity editor
        // https://github.com/zeromq/netmq/issues/526
        AsyncIO.ForceDotNet.Force();
        NetMQConfig.ManualTerminationTakeOver();
        NetMQConfig.ContextCreate(true);

        string subport_p1;
        Debug.Log("Connect to the server: " + IPHeader + PORT + ".");
        var requestSocket = new RequestSocket(IPHeader + PORT);
        requestSocket.SendFrame("SUB_PORT");
        bool is_connected = requestSocket.TryReceiveFrameString(timeout_p1, out subport_p1);
        requestSocket.Close();

        if (is_connected)
        {
            // 
            var subscriberSocket_p1 = new SubscriberSocket(IPHeader + subport_p1);
            subscriberSocket_p1.Subscribe(ID);
            //subscriberSocket_p1.Subscribe("fixations");

            // var subscriberSocket_p2 = new SubscriberSocket(">tcp://192.168.25.50" + subport_p1);
            // subscriberSocket_p1.Subscribe(ID);
            // //subscriberSocket_p1.Subscribe("fixations");


            var msg_p1 = new NetMQMessage();
            while ((is_connected || failed_count_secs <20) && stop_thread_ == false)
            {
               // Debug.Log("Receive a multipart message.");
                is_connected = subscriberSocket_p1.TryReceiveMultipartMessage(timeout_p1, ref (msg_p1));
                if (is_connected)
                {
                    //Debug.Log("Unpack a received multipart message.");
                    try
                    {
                        //Debug.Log(msg[0].ConvertToString());
                        var message = MsgPack.Unpacking.UnpackObject(msg_p1[1].ToByteArray());
                        MsgPack.MessagePackObject mmap = message.Value;
                        lock (thisLock_P1)
                        {
                            itemData = JsonMapper.ToObject(mmap.ToString());

                        }
                        //Debug.Log("p1"+message);
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
            subscriberSocket_p1.Close();
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
        lock (thisLock_P1) stop_thread_ = true;
        client_thread_P1.Join();
        Debug.Log("Quit the thread.");
    }
}
