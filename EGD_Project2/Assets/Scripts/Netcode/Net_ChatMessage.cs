using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_ChatMessage : NetMessage
{
    // 0-8 Op Code
    // 8-256 String message
    public FixedString128Bytes ChatMessage {  get; set; }
    public Net_ChatMessage()
    {
        Code = OpCode.CHAT_MESSAGE;
    }
    public Net_ChatMessage(DataStreamReader reader)
    {
        Code = OpCode.CHAT_MESSAGE;
        Deserialize(reader);
    }
    public Net_ChatMessage(string msg)
    {
        Code = OpCode.CHAT_MESSAGE;
        ChatMessage = msg;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteFixedString128(ChatMessage);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        ChatMessage = reader.ReadFixedString128();
    }

    public override void ReceivedOnClient()
    {
        Debug.Log("Client::" + ChatMessage);
    }
    public override void ReceivedOnServer(ServerManager server)
    {
        Debug.Log("Server::" + ChatMessage);
    }
}
