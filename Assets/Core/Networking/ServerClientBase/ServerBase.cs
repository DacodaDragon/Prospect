﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Basic Unet Server with Reliable and Unreliable channel
/// </summary>
public abstract class ServerBase : MonoBehaviour
{
    private List<int> m_connections;
    private int m_hostID;
    private int m_ChannelReliable;
    private int m_ChannelUnreliable;

    private byte m_error;

    private bool m_running;

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
        m_ChannelReliable   = config.AddChannel(QosType.Reliable);

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
        HandleConnect(connectionid);
    }

    /// <summary>
    /// Handles incoming disconnection requests
    /// </summary>
    /// <param name="connectionid">id of the connection</param>
    private void HandleDisconnect(int connectionid)
    {
        m_connections.Remove(connectionid);
        HandleDisconnect(connectionid);
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

    public abstract void OnConnection(int connectionID);
    public abstract void Ondisconnect(int connectionID);
    public abstract void OnDataRecieved(int clientID, byte[] buffer, int size);
}