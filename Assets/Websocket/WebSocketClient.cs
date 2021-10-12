using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WebSocketClient : MonoBehaviour
{
    WebSocket ws;

    // Start is called before the first frame update
    void Start()
    {
        ws = new WebSocket("ws://192.168.0.103:8080");
        ws.Connect();
        ws.OnMessage += (sender, e) => {
            Debug.Log("Message received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
        };

    }

    // Update is called once per frame
    void Update()
    {
        if(ws == null) {
            Debug.Log("No server connected to websocket client");
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            ws.Send("Hello");
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            ws.Send("GoodBye");
        }
    }
}
