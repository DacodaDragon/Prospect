
using UnityEngine;
using UnityEngine.Networking;
public class Client : MonoBehaviour
{
    private const int MAX_CONNECTIONS = 100;
    private int m_HostID;
    private int m_WebHostID;

    private int m_ReliableChannel;
    private int m_UnreliableChannel;

    private bool m_IsConnected = false;

    private int m_connectionID;

    private byte m_Error;

    public void Connect(ConnectData connection)
    {
        NetworkTransport.Init();

        ConnectionConfig netConfig = new ConnectionConfig();
        m_ReliableChannel = netConfig.AddChannel(QosType.Reliable);
        m_UnreliableChannel = netConfig.AddChannel(QosType.Unreliable);

        HostTopology Topology = new HostTopology(netConfig, MAX_CONNECTIONS);
        m_HostID = NetworkTransport.AddHost(Topology, 0);

        m_connectionID = NetworkTransport.Connect(m_HostID, connection.ip, connection.port, 0, out m_Error);

        m_IsConnected = true;
    }
}