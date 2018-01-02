using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiskTestClient : ClientBase
{
    void Start()
    {
        Connect(5010,"127.0.0.1",1000);
    }

    public void Update()
    {
        if (IsRunning)
            HandleConnections();
    }

    protected override void OnDataRecieved(int clientID, byte[] buffer, int size)
    {
        string message = System.Text.Encoding.Unicode.GetString(buffer, 0, size);
        Debug.Log(string.Format("client {0}: {1}", clientID, message));
    }
}

