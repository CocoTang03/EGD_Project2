using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatMessageTest : MonoBehaviour
{
    [SerializeField] private TMP_InputField chatInput;

    public void OnSubmitClick()
    {
        Net_ChatMessage msg = new Net_ChatMessage(chatInput.text);
        FindObjectOfType<Client>().SendToServer(msg);
        GameObject.Find("ErrorMsg").GetComponent<TextMeshProUGUI>().text = chatInput.text;
    }
}
