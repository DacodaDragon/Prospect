using UnityEngine;
using Game.Networking.Client;

namespace Game.Networking.Test.Client
{
    public class RiskTestClient : ClientBase
    {
        public delegate void StringMessage(string message);
        public StringMessage OnMessageRecieved;

        public void Update()
        {
            if (IsRunning)
                HandleConnections();
        }

        protected override void OnDataRecieved(int clientID, byte[] buffer, int size)
        {
            string message = System.Text.Encoding.UTF8.GetString(buffer, 0, size);
            Debug.Log(string.Format("client {0}: {1}", clientID, message));
            if (OnMessageRecieved != null)
                OnMessageRecieved.Invoke(string.Format("Server {0}: {1}", clientID, message));
        }
    }
}
