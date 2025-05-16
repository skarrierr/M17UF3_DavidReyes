using UnityEngine;
using Unity.Netcode;

public class PlayerNetworkController : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            CameraManager camManager = FindObjectOfType<CameraManager>();
            if (camManager != null)
            {
                camManager.AssignTarget(gameObject);
            }
        }
    }
}
