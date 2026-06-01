using UnityEngine;

public class ServerBootstrap : MonoBehaviour
{
    private Server _server;

    private void Awake()
    {
        _server = new Server();    
    }
}
