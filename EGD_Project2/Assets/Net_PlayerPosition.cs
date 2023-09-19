using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_PlayerPosition : NetMessage
{
    // 0-8 Op Code
    // 8-256 String message
    public int PlayerId { get; set; }
    public float PlayerX { get; set; }
    public float PlayerY { get; set; }
    public float PlayerZ { get; set; }
    public Net_PlayerPosition()
    {
        Code = OpCode.PLAYER_POSITION;
    }
    public Net_PlayerPosition(DataStreamReader reader)
    {
        Code = OpCode.PLAYER_POSITION;
        Deserialize(reader);
    }
    public Net_PlayerPosition(int playerId, float x, float y, float z)
    {
        Code = OpCode.PLAYER_POSITION;
        PlayerId = playerId;
        PlayerX = x; PlayerY = y; PlayerZ = z;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(PlayerId);
        writer.WriteFloat(PlayerX); 
        writer.WriteFloat(PlayerY);
        writer.WriteFloat(PlayerZ);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        PlayerId = reader.ReadInt();
        PlayerX = reader.ReadFloat();
        PlayerY = reader.ReadFloat();
        PlayerZ = reader.ReadFloat();
    }

    public override void ReceivedOnClient()
    {
        Debug.Log("Client::" + PlayerId + "::" + PlayerX + "::" + PlayerY + "::" + PlayerZ);
    }
    public override void ReceivedOnServer(ServerManager server)
    {
        Debug.Log("Server::" + PlayerId + "::" + PlayerX + "::" + PlayerY + "::" + PlayerZ);
        server.BroadCast(this);
    }
}
