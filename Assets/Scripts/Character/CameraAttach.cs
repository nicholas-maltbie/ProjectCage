using Mirror;
using UnityEngine;

namespace Scripts.Character
{
    /// <summary>
    /// Attach the camera to this game object on connect
    /// Detach on disconnect
    /// </summary>
    public class CameraAttach : NetworkBehaviour
    {
        public override void OnStartClient()
        {
            if (isLocalPlayer)
            {
                Camera.main.transform.parent = transform;
                Camera.main.transform.localPosition = new Vector3(0, 0, Camera.main.transform.position.z);
            }
        }
        public override void OnStopClient()
        {
            if (isLocalPlayer)
            {
                if(Camera.main)
                {
                    Camera.main.transform.parent = null;
                    Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
                }
            }
        }
    }
}