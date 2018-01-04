using UnityEngine;
using Game.Networking.Server;

namespace Game.Networking.Test.Server
{
    public class ProspectTestServer : ServerBase
    {

        public delegate void StringMessage(string message);
        public StringMessage OnMessageRecieved;

        public void Update()
        {
            if (IsRunning)
                HandleConnections();
        }

        protected override void OnConnection(int connectionID)
        {
            Debug.Log(string.Format("client {0}: has connected.", connectionID));
            if (OnMessageRecieved != null)
                OnMessageRecieved.Invoke(string.Format("client {0}: has connected.", connectionID));
        }

        protected override void Ondisconnect(int connectionID)
        {
            Debug.Log(string.Format("client {0}:  has disconnected.", connectionID));
            if (OnMessageRecieved != null)
                OnMessageRecieved.Invoke(string.Format("client {0}: has connected.", connectionID));
        }

        protected override void OnDataRecieved(int clientID, byte[] buffer, int size)
        {
            string message = System.Text.Encoding.UTF8.GetString(buffer, 0, size);

            string formattedMessage = string.Format("client {0}: {1}", clientID, message);
            Debug.Log(formattedMessage);

            if (OnMessageRecieved != null)
                OnMessageRecieved.Invoke(formattedMessage);
        }
    }
}