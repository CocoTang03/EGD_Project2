using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendPosition : MonoBehaviour
{
    private float lastSend;
    private Client client;
    // Start is called before the first frame update
    void Start()
    {
        client = FindObjectOfType<Client>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastSend > 1.0f)
        {
            Net_PlayerPosition ps = new Net_PlayerPosition(123321, 
                transform.position.x,
                transform.position.y, 
                transform.position.z);
            client.SendToServer(ps);
            lastSend = Time.time;
        }
    }
}
