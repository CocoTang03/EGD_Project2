using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using System;
using UnityEditor;

public class Client : MonoBehaviour
{
    public NetworkDriver driver;
    private NetworkConnection connection;

    public string ipAddress = "192.168.1.122";
    public ushort port = 8000;

    //private bool isActive = false;
    //private const float keelAliveTickRate = 20.0f;
    //private float lastKeepAlive;

    //public Action connectionDropped;

    void Start(){ Init(); }
    void Update() { UpdateClient(); }
    void OnDestroy() { Shutdown(); }
    public void Init()
    {
        driver = NetworkDriver.Create();
        connection = default(NetworkConnection);
        //NetworkEndPoint endpoint = NetworkEndPoint.Parse(ipAddress,port);
        NetworkEndPoint endpoint = NetworkEndPoint.LoopbackIpv4;
        endpoint.Port = port;
        connection = driver.Connect(endpoint);
    }

    public void UpdateClient()
    {
        driver.ScheduleUpdate().Complete();
        CheckAlive();
        UpdateMessagePump();
    }
    public void Shutdown()
    {
        driver.Dispose();
    }

    private void CheckAlive()
    {
        if (!connection.IsCreated)
        {
            Debug.Log("Something went wrong, lost connection to server");
        }
    }
    protected virtual void UpdateMessagePump()
    {
        DataStreamReader stream;

        NetworkEvent.Type cmd;
        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("We are now connected to the server");
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                OnData(stream);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client disconnected from server");
                connection = default(NetworkConnection);
            }
        }
    }

    public virtual void SendToServer(NetMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);

        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
    public virtual void OnData(DataStreamReader stream)
    {
        NetMessage msg = null;
        var opCode = (OpCode)stream.ReadByte();
        switch (opCode)
        {
            case OpCode.CHAT_MESSAGE: msg = new Net_ChatMessage(stream); break;
            case OpCode.PLAYER_POSITION: msg = new Net_PlayerPosition(stream); break;
            default:
                Debug.Log("Message received had no OpCode");
                break;

        }
        msg.ReceivedOnClient();
    }
}
