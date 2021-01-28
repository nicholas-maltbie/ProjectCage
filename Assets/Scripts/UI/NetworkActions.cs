using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    /// <summary>
    /// Actions to control the network manager via UI buttons
    /// </summary>
    public class NetworkActions : MonoBehaviour
    {
        /// <summary>
        /// Network manager to control network state
        /// </summary>
        public NetworkManager manager { get; private set; }

        /// <summary>
        /// Address to connect to
        /// </summary>
        public InputField connectAddress;

        public bool Initialized()
        {
            return manager != null;
        }

        public void StartHost()
        {
            // Server + Client
            // Make sure to not host on web player, web player can't host I think...
            if (Application.platform != RuntimePlatform.WebGLPlayer && Initialized())
            {
                manager.StartHost();
            }
        }

        public void StartClient()
        {
            if (Initialized())
            {
                manager.networkAddress = connectAddress.text.Trim();
                manager.StartClient();
            }
        }

        public void StartServer()
        {
            if (Initialized())
            {
                manager.StartServer();
            }
        }

        public void StopClientConnecting()
        {
            if (Initialized() && NetworkClient.active)
            {
                manager.StopClient();
            }
        }

        public void StopClient()
        {
            if (Initialized())
            {
                if (NetworkServer.active && NetworkClient.isConnected)
                {
                    manager.StopHost();
                }
                // stop client if client-only
                else if (NetworkClient.isConnected)
                {
                    manager.StopClient();
                }
                // stop server if server-only
                else if (NetworkServer.active)
                {
                    manager.StopServer();
                }
            }
        }

        public void Update()
        {
            // Check if manager is attached, if not find a network manager and attach it
            if (!Initialized())
            {
                manager = GameObject.FindObjectOfType<NetworkManager>();
            }
        }

    }
}