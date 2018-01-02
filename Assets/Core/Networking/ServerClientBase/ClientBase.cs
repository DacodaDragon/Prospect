using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Basic Unet Server with Reliable and Unreliable channel
/// </summary>
public abstract class ClientBase : MonoBehaviour
{
    private int m_hostID;
    private int m_ConnectionID;
    private int m_ChannelReliable;
    private int m_ChannelUnreliable;

    private byte m_error;

    private bool m_running;

    /// <summary>
    /// Starts the server
    /// </summary>
    public void Connect(int port, string ip, int MaxConnections)
    {
        // Initialize NetworkTransport (Unet)
        NetworkTransport.Init();

        ConnectionConfig config = new ConnectionConfig();
        m_ChannelUnreliable = config.AddChannel(QosType.Unreliable);
        m_ChannelReliable = config.AddChannel(QosType.Reliable);

        HostTopology topology = new HostTopology(config, MaxConnections);
        m_hostID = NetworkTransport.AddHost(topology, port);

        m_ConnectionID = NetworkTransport.Connect(
            m_hostID, ip, port, 0, out m_error );

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
            case NetworkEventType.DataEvent:
                HandleRecievedData(connectionID, recievedBuffer, bufferSize);
                break;
        }
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

    public abstract void OnDataRecieved(int clientID, byte[] buffer, int size);
}