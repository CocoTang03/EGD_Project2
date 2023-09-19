using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using System;
using UnityEditor;
using Unity.VisualScripting;
using TMPro;
//using UnityEditor.MemoryProfiler;

public class ServerManager : MonoBehaviour
{
    public NetworkDriver driver;
    protected NativeList<NetworkConnection> connections;

    //private bool isActive = false;
    //private const float keelAliveTickRate = 20.0f;
    //private float lastKeepAlive;

    //public Action connectionDropped;
    //public string ipAddress = "127.0.0.1";
    public ushort port = 8000;

    private void Start() { Init(); }
    private void Update() { UpdateServer(); }
    private void OnDestroy() { Shutdown(); }

    public virtual void Init()
    {
        driver = NetworkDriver.Create();
        NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = port;

        if(driver.Bind(endpoint) != 0)
        {
            Debug.Log("Unable to bind on port " + endpoint.Port);
            return;
        }
        else
        {
            driver.Listen();
            Debug.Log("Currently listening on port " + endpoint.Port);
        }
        connections = new NativeList<NetworkConnection>(4, Allocator.Persistent);
        //isActive = true;
    }
    public virtual void Shutdown()
    {
        if(driver.IsCreated)
        {
            driver.Dispose();
            connections.Dispose();
        }
    }

    public virtual void UpdateServer()
    {
        driver.ScheduleUpdate().Complete();
        CleanupConnections();
        AcceptNewConnections();
        UpdateMessagePump();
    }

    private void CleanupConnections() 
    { 
        for(int i = 0; i < connections.Length; i++)
        {
            if (!connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                --i;
            }

        }
    }
    private void AcceptNewConnections() {
        NetworkConnection c;
        while ((c = driver.Accept()) != default(NetworkConnection))
        {
            connections.Add(c);
            Debug.Log("Accepted a connection");
            GameObject.Find("ErrorMsg").GetComponent<TextMeshProUGUI>().text = "Accepted a connection";
        }
    }
    protected virtual void UpdateMessagePump() {
        DataStreamReader stream;
        for(int i = 0; i < connections.Length; i++)
        {
            NetworkEvent.Type cmd;
            while((cmd = driver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty) 
            {
                if(cmd == NetworkEvent.Type.Data)
                {
                    OnData(stream);
                }
                else if(cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    connections[i] = default(NetworkConnection);
                }
            }
        }
    }
    public virtual void OnData(DataStreamReader stream)
    {
        NetMessage msg = null;
        var opCode = (OpCode)stream.ReadByte();
        switch(opCode)
        {
            case OpCode.CHAT_MESSAGE: msg = new Net_ChatMessage(stream); break;
            case OpCode.PLAYER_POSITION: msg = new Net_PlayerPosition(stream); break;
            default:
                Debug.Log("Message received had no OpCode");
                break;

        }
        msg.ReceivedOnServer(this);
    }
    // Send to all clients 
    public virtual void BroadCast(NetMessage msg)
    {
        for (int i = 0; i < connections.Length; ++i)
        {
            if (connections[i].IsCreated)
            {
                SendToClient(connections[i], msg);
            }
        }
    }
    // Send to a specific client
    public virtual void SendToClient(NetworkConnection connection, NetMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);

        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
}
