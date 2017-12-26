using UnityEngine;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour {
    [SerializeField] private InputField m_port;
    [SerializeField] private InputField m_ip;
    [SerializeField] private Button m_connect;
    [SerializeField] private Client m_client;

    public void Start()
    {
        m_connect.onClick.AddListener(TryConnect);
    }

    private void TryConnect()
    {
        int port = int.Parse(m_port.text);
        string ip = m_ip.text;

        m_client.Connect(new ConnectData(port, ip));
    }
}