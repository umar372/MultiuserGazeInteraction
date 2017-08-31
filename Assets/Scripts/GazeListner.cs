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
using System.IO;

using System.Net;
using System.Net.Sockets;


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

    public JsonData itemData_1;
    public JsonData itemData_2;
    public Text normPosText, confidenceText, isOneSurface, blinkDetected;

    Thread client_thread_P1,client_thread_P2;
    private System.Object thisLock_P1 = new System.Object();
    private System.Object thisLock_P2 = new System.Object();
    bool stop_thread_ = false;

    public string IP_1;
    public string PORT_1;
    public string ID_1;

    public string IP_2;
    public string PORT_2;
    public string ID_2;

    bool isDetectGesture, startedDetection, boolAgainBlink;
    float timer;
    int numOfBlinks;

    private double xpos_1 = -1;
    private double ypos_1 = -1;
    public float confidence_1 = 1;
    public bool isOnSurface_1;

    private double xpos_2 = -1;
    private double ypos_2 = -1;
    public float confidence_2 = 1;
    public bool isOnSurface_2;

    public double xposP1 = -1;
    public double yposP1 = -1;

    public double xposP2 = -1;
    public double yposP2 = -1;

    public float confidence = -1;
    public bool isOnSurface;


    private double xPosSumP1 = 0, yPosSumP1 = 0, xPosSumP2 = 0, yPosSumP2 = 0;
    private int numOfSumsP1 = 0, numOfSumsP2 = 0;

    //Variables for stabilising the gaze values after significant drop of confidence
    private int skipValuesP1 = 5, skipValuesP2 = 5;
    private bool beforeDropConfP1 = false, beforeDropConfP2 = false;
    private double tmpXP1 = 0, tmpXP2 = 0, tmpYP1 = 0, tmpYP2 = 0;



    int failed_count_secs = 0, failed_count_secs2=0;


    GazeData.GazeData2D data_ = new GazeData.GazeData2D();
    SubscriberSocket subscriberSocket_1, subscriberSocket_2;


     Queue<Vector2> quPlayer1, quPlayer2;

    bool toggleRead;


    public void get_Data_Player1()
    {
        lock (thisLock_P1)
        {
            if (itemData_1 != null)
            {

                if (itemData_1.Keys.Contains("gaze_on_srf"))
                {
                    if (itemData_1["gaze_on_srf"].Count > 0 && confidence_1 > 0.6 && skipValuesP1 == 5)
                    {

                        tmpXP1 = Math.Round((double)itemData_1["gaze_on_srf"][0]["norm_pos"][0], 2);
                        tmpYP1 = Math.Round((double)itemData_1["gaze_on_srf"][0]["norm_pos"][1], 2);
                        //normPosText.text = "Norm " + xpos_1 + " , " + ypos_1;
                         if(tmpXP1 <= 1.0 && tmpXP1 >= 0)
                             xpos_1 = tmpXP1;
                         if(tmpYP1 <= 1.0 && tmpYP1 >= 0)
                             ypos_1 = tmpYP1;
                     }
                     else
                     {
                         if(skipValuesP1 == 5)
                         {
                             skipValuesP1 = 0;
                             beforeDropConfP1 = true;
                         }
                         skipValuesP1++;
                     }
                    }
                    if (itemData_1["gaze_on_srf"].Count > 0)
                    {
                        string conf = itemData_1["gaze_on_srf"][0]["confidence"].ToString();
                        confidence_1 = float.Parse(conf);
                        //confidenceText.text = "conf " + confidence_1;
                        isOnSurface_1 = (bool)itemData_1["gaze_on_srf"][0]["on_srf"];
                       // isOneSurface.text = "IsOnSurf " + (bool)isOneSurface;
                    }
                }
            }
        
        
    }

    public void get_Data_Player2()
    {
        lock (thisLock_P1)
        {
            if (itemData_2 != null)
            {

                if (itemData_2.Keys.Contains("gaze_on_srf"))
                {
                    if (itemData_2["gaze_on_srf"].Count > 0 && confidence_2 > 0.6 && skipValuesP2 == 5)
                    {

                        tmpXP2 = Math.Round((double)itemData_2["gaze_on_srf"][0]["norm_pos"][0], 2);
                        tmpYP2 = Math.Round((double)itemData_2["gaze_on_srf"][0]["norm_pos"][1], 2);
                        // normPosText.text = "Norm " + xpos_2 + " , " + ypos_2;

                        if (tmpXP2 <= 1.0 && tmpXP2 >= 0)
                             xpos_2 = tmpXP2;
                         if (tmpYP2 <= 1.0 && tmpYP2 >= 0)
                             ypos_2 = tmpYP2;

                     }
                     else
                     {
                         if (skipValuesP2 == 5)
                         {
                             skipValuesP2 = 0;
                             beforeDropConfP2 = true;
                         }
                         skipValuesP2++;
                     }
                    }
                    if (itemData_2["gaze_on_srf"].Count > 0)
                    {
                        string conf = itemData_2["gaze_on_srf"][0]["confidence"].ToString();
                        confidence_2 = float.Parse(conf);
                        //confidenceText.text = "conf " + confidence_2;
                        isOnSurface_2 = (bool)itemData_2["gaze_on_srf"][0]["on_srf"];
                       // isOneSurface.text = "IsOnSurf " + (bool)isOneSurface;
                    }
                }
            }
        
    }
    // Use this for initialization
    void Start () {
        failed_count_secs = 0;
        Debug.Log("Start a request thread.");
        client_thread_P1 = new Thread(MQClientForPlayers);
        client_thread_P1.Start();

        quPlayer1 = new Queue<Vector2>();
        quPlayer2 = new Queue<Vector2>();

    }

    // Update is called once per frame
    void Update()
    {
        get_Data_Player1();
        get_Data_Player2();


        if (quPlayer1.Count < 30)
        {
            quPlayer1.Enqueue(new Vector2((float)xpos_1, (float)ypos_1));

        }
        else {
            quPlayer1.Enqueue(new Vector2((float)xpos_1, (float)ypos_1));
            Vector2[] temp = new Vector2[quPlayer1.Count];
            quPlayer1.CopyTo(temp, 0);
            xPosSumP1 = 0;
            yPosSumP1 = 0;
            if(beforeDropConfP1)
            {
                for(int i = 25; i < 30; i++)
                {
                    temp[i].x = temp[i - 5].x;
                    temp[i].y = temp[i - 5].y;
                }
                beforeDropConfP1 = false;
            }
            for (int i = 0; i < temp.Length; i++)
            {
                xPosSumP1 += temp[i].x;
                yPosSumP1 += temp[i].y;

            }
            xposP1 = xPosSumP1 / temp.Length;
            yposP1 = yPosSumP1 / temp.Length;
            quPlayer1.Dequeue();

        }

        ///////////Smoothing player 2
        if (quPlayer2.Count < 30)
        {
            quPlayer2.Enqueue(new Vector2((float)xpos_2, (float)ypos_2));

        }
        else
        {
            quPlayer2.Enqueue(new Vector2((float)xpos_2, (float)ypos_2));
            Vector2[] temp2 = new Vector2[quPlayer2.Count];
            quPlayer2.CopyTo(temp2, 0);
            xPosSumP2 = 0;
            yPosSumP2 = 0;
            if (beforeDropConfP2)
            {
                for (int i = 25; i < 30; i++)
                {
                    temp2[i].x = temp2[i - 5].x;
                    temp2[i].y = temp2[i - 5].y;
                }
                beforeDropConfP2 = false;
            }
            for (int i = 0; i < temp2.Length; i++)
            {
                xPosSumP2 += temp2[i].x;
                yPosSumP2 += temp2[i].y;

            }
            xposP2 = xPosSumP2 / temp2.Length;
            yposP2 = yPosSumP2 / temp2.Length;
            quPlayer2.Dequeue();

        }
    }




    void MQClientForPlayers()
    {

        string IPHeader_1 = ">tcp://" + IP_1 + ":";

        var timeout = new System.TimeSpan(0, 0, 1);

        AsyncIO.ForceDotNet.Force();
        NetMQConfig.ManualTerminationTakeOver();
        NetMQConfig.ContextCreate(true);

        string subport_1;
        string subport_2;
        Debug.Log("Connect to the server: " + IPHeader_1 + PORT_1 + ".");
        var requestSocket_1 = new RequestSocket(IPHeader_1 + PORT_1);
        requestSocket_1.SendFrame("SUB_PORT");
        bool is_connected_1 = requestSocket_1.TryReceiveFrameString(timeout, out subport_1);
        requestSocket_1.Close();

        client_thread_P2 = new Thread(MQClientForPlayer2);
        client_thread_P2.Start();
        if (is_connected_1)
        {

            subscriberSocket_1 = new SubscriberSocket(IPHeader_1 + subport_1);
            //  var subscriberSocket_2 = new SubscriberSocket(IPHeader_2 + subport_2);
            subscriberSocket_1.Subscribe(ID_1);
            // subscriberSocket_2.Subscribe(ID_2);

            var msg_1 = new NetMQMessage();
            var msg_2 = new NetMQMessage();
            while ((is_connected_1 || failed_count_secs < 20) && stop_thread_ == false)
            {

                // Debug.Log("Receive a multipart message.");
                is_connected_1 = subscriberSocket_1.TryReceiveMultipartMessage(timeout, ref (msg_1));
                //  is_connected_2 = subscriberSocket_2.TryReceiveMultipartMessage(timeout, ref (msg_2));
                if (is_connected_1)
                {
                    //Debug.Log("Unpack a received multipart message.");
                    try
                    {
                        //Debug.Log(msg[0].ConvertToString());
                        var message_1 = MsgPack.Unpacking.UnpackObject(msg_1[1].ToByteArray());
                        //var message_2 = MsgPack.Unpacking.UnpackObject(msg_2[1].ToByteArray());
                        MsgPack.MessagePackObject mmap_1 = message_1.Value;
                        // MsgPack.MessagePackObject mmap_2 = message_2.Value;
                        lock (thisLock_P1)
                        {
                            itemData_1 = JsonMapper.ToObject(mmap_1.ToString());
                            //itemData_2 = JsonMapper.ToObject(mmap_2.ToString());

                        }
                        //Debug.Log("p1 " + message_1);
                        //Debug.Log("p2 " + message_2);
                        failed_count_secs = 0;
                    }
                    catch
                    {
                        Debug.Log("Failed to unpack.");
                    }
                }
                else
                {
                    Debug.Log("Failed to receive a message p1");
                    Thread.Sleep(1000);
                    failed_count_secs += 1;
                }
            }
            subscriberSocket_1.Close();
            subscriberSocket_2.Close();
            //   subscriberSocket_2.Close();
        }
        else
        {
            Debug.Log("Failed to connect the server p1");
        }

        // Necessary to handle this NetMQ issue on Unity editor
        // https://github.com/zeromq/netmq/issues/526
        Debug.Log("ContextTerminate p1");

        NetMQConfig.ContextTerminate();

    }

    void MQClientForPlayer2()
    {

        string IPHeader_2 = ">tcp://" + IP_2 + ":";
        var timeout = new System.TimeSpan(0, 0, 1);



        string subport_2;
        Debug.Log("Connect to the server: " + IPHeader_2 + PORT_2 + ".");


        var requestSocket_2 = new RequestSocket(IPHeader_2 + PORT_2);
        requestSocket_2.SendFrame("SUB_PORT");
        bool is_connected_2 = requestSocket_2.TryReceiveFrameString(timeout, out subport_2);
        requestSocket_2.Close();

        if (is_connected_2)
        {
            // 
            subscriberSocket_2 = new SubscriberSocket(IPHeader_2 + subport_2);
            subscriberSocket_2.Subscribe(ID_2);

            var msg_2 = new NetMQMessage();
            while ((is_connected_2 || failed_count_secs2 < 20) && stop_thread_ == false)
            {
                // Debug.Log("Receive a multipart message.");
                is_connected_2 = subscriberSocket_2.TryReceiveMultipartMessage(timeout, ref (msg_2));
                if (is_connected_2)
                {
                    //Debug.Log("Unpack a received multipart message.");
                    try
                    {
                        //Debug.Log(msg[0].ConvertToString());
                        var message_2 = MsgPack.Unpacking.UnpackObject(msg_2[1].ToByteArray());
                        MsgPack.MessagePackObject mmap_2 = message_2.Value;
                        lock (thisLock_P2)
                        {
                            itemData_2 = JsonMapper.ToObject(mmap_2.ToString());

                        }
                       // Debug.Log("p2 " + message_2);

                        failed_count_secs2 = 0;
                    }
                    catch
                    {
                        Debug.Log("Failed to unpack.");
                    }
                }
                else
                {
                    Debug.Log("Failed to receive a message p2");
                    Thread.Sleep(1000);
                    failed_count_secs2 += 1;
                }
            }
            subscriberSocket_1.Close();
            subscriberSocket_2.Close();
        }
        else
        {
            Debug.Log("Failed to connect the server p2");
        }

        // Necessary to handle this NetMQ issue on Unity editor
        // https://github.com/zeromq/netmq/issues/526
        Debug.Log("ContextTerminate p2");


    }
    void OnApplicationQuit()
    {
        lock (thisLock_P1) stop_thread_ = true;
        client_thread_P1.Join();

        lock (thisLock_P2) stop_thread_ = true;
        client_thread_P2.Join();
        Debug.Log("Quit the thread.");
    }
}
