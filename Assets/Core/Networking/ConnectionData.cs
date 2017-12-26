[System.Serializable]
public struct ConnectData {
    public readonly int port;
    public readonly string ip;

    public ConnectData(int port, string ip)
    {
        this.port = port;
        this.ip = ip;
    }
}
