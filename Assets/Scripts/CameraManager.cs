using UnityEngine;
using Unity.Netcode;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public RaycastLookAt raycastLookAt;
    public CameraController cameraController;
    public ObjectFollower objectFollower;

    public void AssignTarget(GameObject player)
    {
        CharacterMover mover = player.GetComponent<CharacterMover>();
        CharacterAnimator animator = player.GetComponentInChildren<CharacterAnimator>();

        if (mover != null)
            mover.cam = mainCamera;

        if (animator != null)
        {
            animator.cam = mainCamera;
            animator.cameraLookAt = raycastLookAt;
        }

        objectFollower.follow = player.transform;
        cameraController.lookAt = player.transform;
    }
}
