using UnityEngine;
using UnityEngine.UI;

public class ServerTestMessenger : MonoBehaviour
{
    [SerializeField] private InputField m_text;
    [SerializeField] private Button m_send;
    [SerializeField] private RiskTestServer m_server;
    [SerializeField] private Text m_textfield;
    string text;

    public void Start()
    {
        m_send.onClick.AddListener(SendNetworkMessage);
        m_text.onEndEdit.AddListener(SendNetworkMessage);
        m_server.OnMessageRecieved += RecieveMessage;
    }

    public void RecieveMessage(string message)
    {
        m_textfield.text += message + '\n';
    }

    public void SendNetworkMessage(string message)
    {
        m_server.SendReliable(message);
        m_textfield.text += "Server: " + message + '\n';
        m_text.text = string.Empty;
    }

    public void SendNetworkMessage()
    {
        m_server.SendReliable(m_text.text);
        m_textfield.text += "Server: " + m_text.text + '\n';
        m_text.text = string.Empty;
    }
}
