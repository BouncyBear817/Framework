/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/03/20 11:17:25
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections;
using System.Text;
using Framework;
using UnityEngine;

public class Example_Network : MonoBehaviour
{
    private string mIp = "127.0.0.1";
    private string mPort = "8808";
    private string mChannelName = "Test";
    private string mMessage;
    private string mPacketMessage;

    private const int EventId = 100;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        MainEntry.Event.Subscribe(Runtime.NetworkConnectedEventArgs.EventId, NetworkConnectedHandler);
    }

    void Connect(string ip, string port, string channelName)
    {
        MainEntry.NetConnector.Connect(ip, int.Parse(port), channelName);
        MainEntry.NetConnector.SetHeartBeatInterval(mChannelName, 20f);
    }

    private void NetworkConnectedHandler(object sender, BaseEventArgs e)
    {
        var eventArgs = e as Runtime.NetworkConnectedEventArgs;
        if (eventArgs != null)
        {
            Debug.Log(eventArgs.NetworkChannel.Name);
            MainEntry.NetConnector.RegisterHandler(mChannelName, EventId, Handler);
        }
    }

    private void Handler(object sender, Packet e)
    {
        var p = e as SCPacketBase;
        if (p != null)
        {
            Debug.Log($"Id : {p.Id}, message : {Utility.Converter.GetString(p.MessageBody)}.");
        }
    }

    void Send(string message)
    {
        MainEntry.NetConnector.Send(mChannelName, EventId, Encoding.UTF8.GetBytes(message));
    }

    void SendPacket(string message)
    {
        var packet = ReferencePool.Acquire<CSTest>();
        packet.MessageBody = Utility.Converter.GetBytes(message);
        MainEntry.NetConnector.Send(mChannelName, packet);
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

        mPacketMessage = GUI.TextField(new Rect(20, 120, 200, 30), mPacketMessage);

        if (GUI.Button(new Rect(240, 120, 100, 30), "SendPacket"))
        {
            SendPacket(mPacketMessage);
        }
    }
}

public class CSTest : CSPacketBase
{
    public override int Id => 200;
}

public class SCTest : SCPacketBase
{
    public override int Id => 200;
}

public class TestHandler : PacketHandlerBase
{
    public override int Id => 200;

    public override void Handle(object sender, Packet packet)
    {
        var p = packet as SCPacketBase;
        if (p != null)
        {
            Debug.Log(Utility.Converter.GetString(p.MessageBody));
        }
    }
}

// public class TestHandler1 : PacketHandlerBase
// {
//     public override int Id => 100;
//
//     public override void Handle(object sender, Packet packet)
//     {
//         var p = packet as SCPacketBase;
//         if (p != null)
//         {
//             Debug.Log(Utility.Converter.GetString(p.MessageBody));
//         }
//     }
// }