using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private PlayerController playerController;
    private CinemachineVirtualCamera gameCam;

    public CameraMove cameraController;
    public Transform[] targetRoomPositions;

    private void Start()
    {
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        
        gameCam = cameraController.GetComponent<CinemachineVirtualCamera>();
        gameCam.Follow = null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(gameCam.Follow == null)
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
        else
        {
            if (other.CompareTag("Player")) // Make sure player has "Player" tag
            {
                gameCam.Follow = null;
                cameraController.MoveCameraToRoom(targetRoomPositions);
            }
        }
    }
}
