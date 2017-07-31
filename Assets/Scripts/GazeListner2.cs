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





public class GazeListner2 : MonoBehaviour
{

    private JsonData itemData;
    public Text normPosText, confidenceText, isOneSurface;

    Thread client_thread2_;
    private System.Object thisLock_ = new System.Object();
    bool stop_thread_ = false;
    public string IP;
    public string PORT;
    public string ID;

    GazeData.GazeData2D data_ = new GazeData.GazeData2D();

    public void get_transform()
    {
        lock (thisLock_)
        {
            if (itemData != null)
            {
                double xpos = -1;
                double ypos = -1;
                if (itemData["gaze_on_srf"].Count > 0)
                {
                    if (itemData["gaze_on_srf"][0]["norm_pos"].Count > 0)
                    {
                        xpos = Math.Round((double)itemData["gaze_on_srf"][0]["norm_pos"][0], 2);
                        ypos = Math.Round((double)itemData["gaze_on_srf"][0]["norm_pos"][1], 2);
                    }
                }
                if (itemData["gaze_on_srf"].Count > 0)
                {
                    isOneSurface.text = "IsOnSurd " + (bool)itemData["gaze_on_srf"][0]["on_srf"];
                    // confidenceText.text = "confid  "+(double)itemData["gaze_on_srf"][0]["confidence"];

                }


                normPosText.text = "Norm " + xpos + " , " + ypos;//["norm_pos"][0] + " , " + itemData["gaze_on_srf"]["norm_pos"][0];
            }


        }
    }
    // Use this for initialization
    void Start()
    {


        Debug.Log("Start a request thread.");
        client_thread2_ = new Thread(NetMQClient);
        client_thread2_.Start();
    }

    // Update is called once per frame
    void Update()
    {
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

            var msg = new NetMQMessage();
            while (is_connected && stop_thread_ == false)
            {
                Debug.Log("Receive a multipart message.");
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

                            //data_ = JsonUtility.FromJson<GazeData.GazeData2D>(mmap.ToString());
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
        client_thread2_.Join();
        Debug.Log("Quit the thread.");
    }
}
