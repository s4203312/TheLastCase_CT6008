using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public CameraMove cameraController;
    public Transform targetRoomPosition; // Assign Room1CamPos, Room2CamPos, etc.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure player has "Player" tag
        {
            cameraController.MoveCameraToRoom(targetRoomPosition);
        }
    }
}
