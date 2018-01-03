using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Game.Networking.Test.Server
{
    public class ServerTestSetup : MonoBehaviour
    {

        [SerializeField] private InputField m_port;
        [SerializeField] private InputField m_conn;
        [SerializeField] private Button m_start;
        [SerializeField] private RiskTestServer m_testServer;
        [SerializeField] private UnityEvent m_OnConnect;

        public void Start()
        {
            m_start.onClick.AddListener(StartServer);
        }

        public void StartServer()
        {
            int port = 1234;
            int conn = 50;

            if (m_port.text != "")
                port = int.Parse(m_port.text);
            if (m_conn.text != "")
                conn = int.Parse(m_conn.text);

            m_testServer.Startup(port, conn);
            m_OnConnect.Invoke();
        }
    }
}