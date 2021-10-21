//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;

[Serializable]
public struct JsonData {
    public int level;
    public float timeElapsed;
    public string LED;
}

public class WebSocketClient : MonoBehaviour {
    JsonData jsonData;
    WebSocket ws;

    // Start is called before the first frame update
    void Start() {
        jsonData = new JsonData();
        jsonData.level = 1;
        jsonData.timeElapsed = 47.5f;
        jsonData.LED = "off";

        ws = new WebSocket("ws://192.168.0.103:7890");
        ws.Connect();
        ws.OnMessage += (sender, e) => {
            Debug.Log("Message received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
        };


        InvokeRepeating("SendMessageToServer", 0, 0.25f);
    }

    void SendMessageToServer() {
        if (ws == null) {
            Debug.Log("No server connected to web socket client");
            return;
        }

        if (Input.GetKey(KeyCode.Space)) {
            jsonData.LED = "on";
        }

        if (Input.GetKey(KeyCode.A)) {
            jsonData.LED = "off";
        }

        string outJson = JsonUtility.ToJson(jsonData);
        ws.Send(outJson);
    }

    private void OnApplicationQuit() {
        ws.Close();
    }
}
