using UnityEngine;
using WebSocketSharp;
using System;

namespace APG {
    [Serializable]
    public struct JsonData {
        public string LED;

        public bool moveUp;
        public bool moveDown;
        public bool moveLeft;
        public bool moveRight;
    }

    public class WebSocketClient : MonoBehaviour {

        private string webSocketAddress = "192.168.0.111:7890";

        JsonData jsonData;
        WebSocket ws;

        private PlayerInputHandler playerInputHandler;

        // Start is called before the first frame update
        public void InitializeWebSocketClient() {
            jsonData = new JsonData();
            jsonData.LED = "off";

            ws = new WebSocket("ws://" + webSocketAddress);
            ws.Connect();
            ws.OnMessage += (sender, e) => {
                Debug.Log("Message received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
            };
        }

        public void SendMessageToServer(String LED, bool up, bool down, bool left, bool right) {
            if (ws == null) {
                Debug.Log("No server connected to web socket client");
                return;
            }

            jsonData.LED = LED;
            jsonData.moveUp = up;
            jsonData.moveDown = down;
            jsonData.moveLeft = left;
            jsonData.moveRight = right;

            string outJson = JsonUtility.ToJson(jsonData);
            ws.Send(outJson);
        }

        private void OnApplicationQuit() {
            ws.Close();
        }
    }
}