/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/03/20 11:17:25
 * Description:
 * Modify Record:
 *************************************************************/

using System.Text;
using Runtime;
using UnityEngine;

public class Example_Network : MonoBehaviour
{
    private string mIp = "127.0.0.1";
    private string mPort = "8808";
    private string mChannelName = "Test";
    private string mMessage;

    void Connect(string ip, string port, string channelName)
    {
        MainEntry.NetConnector.Connect(ip, int.Parse(port), channelName);
        MainEntry.NetConnector.SetHeartBeatInterval(mChannelName, 20f);
    }

    void Send(string message)
    {
        MainEntry.NetConnector.Send(mChannelName, 1, Encoding.UTF8.GetBytes(message));
    }

    void Close()
    {
        MainEntry.NetConnector.Close(mChannelName);
    }

    // Update is called once per frame
    void OnGUI()
    {
        mIp = GUI.TextField(new Rect(20, 20, 100, 30), mIp);
        mPort = GUI.TextField(new Rect(140, 20, 50, 30), mPort);
        mChannelName = GUI.TextField(new Rect(210, 20, 50, 30), mChannelName);
        if (GUI.Button(new Rect(280, 20, 100, 30), "Connect"))
        {
            Connect(mIp, mPort, mChannelName);
        }

        if (GUI.Button(new Rect(400, 20, 100, 30), "Close"))
        {
            Close();
        }

        mMessage = GUI.TextField(new Rect(20, 70, 200, 30), mMessage);

        if (GUI.Button(new Rect(240, 70, 100, 30), "Send"))
        {
            Send(mMessage);
        }
    }
}