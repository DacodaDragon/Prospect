using UnityEngine;

public class RiskTestServer : ServerBase
{
    void Start()
    {
        Startup(5010, 1000);
    }

    public void Update()
    {
        if (IsRunning)
            HandleConnections();
    }

    protected override void OnConnection(int connectionID)
    {
        Debug.Log(string.Format("client {0}: has connected.",connectionID));
    }

    protected override void Ondisconnect(int connectionID)
    {
        Debug.Log(string.Format("client {0}:  has disconnected.",connectionID));
    }

    protected override void OnDataRecieved(int clientID, byte[] buffer, int size)
    {
        string message = System.Text.Encoding.Unicode.GetString(buffer, 0, size);
        Debug.Log(string.Format("client {0}: {1}", clientID, message));
    }
}
