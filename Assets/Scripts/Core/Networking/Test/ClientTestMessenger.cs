using UnityEngine;
using UnityEngine.UI;

namespace Game.Networking.Test.Client
{
    public class ClientTestMessenger : MonoBehaviour
    {
        [SerializeField] private InputField m_text;
        [SerializeField] private Button m_send;
        [SerializeField] private ProspectTestClient m_client;
        [SerializeField] private Text m_textfield;

        public void Start()
        {
            m_send.onClick.AddListener(SendNetworkMessage);
            m_text.onEndEdit.AddListener(SendNetworkMessage);
            m_client.OnMessageRecieved += (string message) =>
            {
                m_textfield.text += message + '\n';
            };
        }

        public void SendNetworkMessage(string message)
        {
            m_client.SendReliable(message);
            m_textfield.text += "Client: " + message + '\n';
            m_text.text = string.Empty;
        }

        public void SendNetworkMessage()
        {
            m_client.SendReliable(m_text.text);
            m_textfield.text += "Client: " + m_text.text + '\n';
            m_text.text = string.Empty;
        }
    }
}