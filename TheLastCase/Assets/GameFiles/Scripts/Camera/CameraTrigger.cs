using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private PlayerController playerController;
    private CinemachineVirtualCamera gameCam;

    public CameraMove cameraController;
    public Transform targetRoomPosition;
    public bool inToRoom = true;

    private void Start()
    {
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        
        gameCam = cameraController.GetComponent<CinemachineVirtualCamera>();
        gameCam.Follow = null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (inToRoom)       //Focus on room
        {
            if (other.CompareTag("Player")) // Make sure player has "Player" tag
            {
                gameCam.Follow = null;
                cameraController.MoveCameraToRoom(targetRoomPosition.position, targetRoomPosition.rotation);
            }
        }
        else               //Follow if corridoors
        {
            if (playerController.isGhostActive)
            {
                gameCam.Follow = GameObject.Find("Ghost").transform;
            }
            else
            {
                gameCam.Follow = GameObject.Find("Player").transform;
            }
        }
    }
}
