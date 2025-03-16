using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public CameraMove cameraController;
    public Transform targetRoomPosition;
    public bool inToRoom = true;

    private void OnTriggerEnter(Collider other)
    {
        var cam = cameraController.GetComponent<CinemachineVirtualCamera>();

        if (inToRoom)       //Focus on room
        {
            if (other.CompareTag("Player")) // Make sure player has "Player" tag
            {
                cam.Follow = null;
                cameraController.MoveCameraToRoom(targetRoomPosition);
            }
        }
        else               //Follow if corridoors
        {
            cam.Follow = GameObject.Find("Player").transform;
        }
    }
}
