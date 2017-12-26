using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private const int MAX_CONNECTIONS = 100;
    private int m_Port = 5010;
    private int m_HostID;
    private int m_WebHostID;

    private int m_ReliableChannel;
    private int m_UnreliableChannel;
    private bool m_IsConnected = false;

    private byte m_Error;

    private void Start()
    {
        NetworkTransport.Init();

        ConnectionConfig netConfig = new ConnectionConfig();
        m_ReliableChannel = netConfig.AddChannel(QosType.Reliable);
        m_UnreliableChannel = netConfig.AddChannel(QosType.Unreliable);

        HostTopology Topology = new HostTopology(netConfig, MAX_CONNECTIONS);
        m_HostID    = NetworkTransport.AddHost         (Topology, m_Port, null);
        m_WebHostID = NetworkTransport.AddWebsocketHost(Topology, m_Port, null);
    }

    private void Update()
    {
        if (m_IsConnected)
            HandleConnection();
    }

    private void HandleConnection()
    {
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;

        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log("A client connected!");
                break;
            case NetworkEventType.DataEvent:
                Debug.Log("Some data was send!");
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("A Client disconnected!");
                break;
        }
    }
}
