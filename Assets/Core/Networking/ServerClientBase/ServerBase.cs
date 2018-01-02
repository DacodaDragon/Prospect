using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Basic Unet Server with Reliable and Unreliable channel
/// </summary>
public abstract class ServerBase : MonoBehaviour
{

    private List<int> m_connections;
    private int m_hostID;
    private int m_channelReliable;
    private int m_ChannelUnreliable;
    private byte m_error;

    private bool m_running;
    public bool IsRunning { get { return m_running; } }

    /// <summary>
    /// Starts the server
    /// </summary>
    /// <param name="port">port to host the server on</param>
    /// <param name="MaxConnections">maximum connections allowed</param>
    public void Startup(int port, int MaxConnections)
    {
        // Initialize NetworkTransport (Unet)
        NetworkTransport.Init();

        ConnectionConfig config = new ConnectionConfig();
        m_ChannelUnreliable = config.AddChannel(QosType.Unreliable);
        m_channelReliable   = config.AddChannel(QosType.Reliable);

        HostTopology topology = new HostTopology(config, MaxConnections);
        m_hostID = NetworkTransport.AddHost(topology, port);

        m_connections = new List<int>();
        m_running = true;
    }

    /// <summary>
    /// Handles connecting, disconnecting and data send by clients.
    /// </summary>
    public void HandleConnections()
    {
        int hostID;
        int connectionID;
        int channelId;
        byte[] recievedBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;

        NetworkEventType recievedData = NetworkTransport.Receive
        (
            out hostID,
            out connectionID,
            out channelId,
            recievedBuffer,
            bufferSize,
            out dataSize,
            out error
        );

        switch (recievedData)
        {
            case NetworkEventType.ConnectEvent:
                HandleConnect(connectionID);
                break;
            case NetworkEventType.DisconnectEvent:
                HandleDisconnect(connectionID);
                break;
            case NetworkEventType.DataEvent:
                HandleRecievedData(connectionID, recievedBuffer, bufferSize);
                break;
        }
    }

    /// <summary>
    /// Handles incoming connection requests
    /// </summary>
    /// <param name="connectionid">id of the connection</param>
    private void HandleConnect(int connectionid)
    {
        m_connections.Add(connectionid);
        OnConnection(connectionid);
    }

    /// <summary>
    /// Handles incoming disconnection requests
    /// </summary>
    /// <param name="connectionid">id of the connection</param>
    private void HandleDisconnect(int connectionid)
    {
        m_connections.Remove(connectionid);
        Ondisconnect(connectionid);
    }

    /// <summary>
    /// Handles Incoming Data
    /// </summary>
    /// <param name="connectionid">id of the connection</param>
    /// <param name="buffer">buffer of bytes containing data</param>
    /// <param name="bufferSize">size of buffer</param>
    private void HandleRecievedData(int connectionid, byte[] buffer, int bufferSize)
    {
        OnDataRecieved(connectionid, buffer, bufferSize);
    }

    protected abstract void OnConnection(int connectionID);
    protected abstract void Ondisconnect(int connectionID);
    protected abstract void OnDataRecieved(int clientID, byte[] buffer, int size);

    public void SendReliable(string message)
    {
        SendMessage(message, m_connections.ToArray());
    }
    public void SendReliable(string message, params int[] client)
    {
        byte[] data = System.Text.Encoding.Unicode.GetBytes(message);
        SendReliable(data, client);
    }
    public void SendReliable(byte[] message, params int[] client)
    {
        for (int i = 0; i < client.Length; i++)
        {
            NetworkTransport.Send
                (
                m_hostID, client[i],
                m_channelReliable,
                message,
                message.Length * sizeof(byte),
                out m_error);
        }
    }
    public void SendUnreliable(string message)
    {
        SendUnreliable(message, m_connections.ToArray());
    }
    public void SendUnreliable(string message, params int[] client)
    {
        byte[] data = System.Text.Encoding.Unicode.GetBytes(message);
        SendReliable(data, client);
    }
    public void SendUnreliable(byte[] message, params int[] client)
    {
        for (int i = 0; i < client.Length; i++)
        {
            NetworkTransport.Send
                (
                m_hostID, client[i],
                m_ChannelUnreliable,
                message,
                message.Length * sizeof(byte),
                out m_error);
        }
    }
}