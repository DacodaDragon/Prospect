using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Game.Networking.Test.Client
{
    public class ClientTestSetup : MonoBehaviour
    {
        [SerializeField] private InputField m_port;
        [SerializeField] private InputField m_conn;
        [SerializeField] private InputField m_ip;
        [SerializeField] private Button m_start;
        [SerializeField] private ProspectTestClient m_testClient;
        [SerializeField] private UnityEvent m_OnConnect;

        public void Start()
        {
            m_start.onClick.AddListener(StartServer);
        }

        public void StartServer()
        {
            int port = 1234;
            int conn = 50;
            string ip = "127.0.0.1";

            if (m_port.text != "")
                port = int.Parse(m_port.text);

            if (m_conn.text != "")
                conn = int.Parse(m_conn.text);

            if (m_ip.text != "")
                ip = m_ip.text;

            m_testClient.Connect(port, ip, conn);
            m_OnConnect.Invoke();
        }
    }
}