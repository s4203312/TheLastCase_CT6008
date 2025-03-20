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
            //Quaternion startRotation = gameCam.transform.rotation;
            //Quaternion endRotation = new Quaternion(86.667f, -90.398f, 0.0151f , 1);

            if (playerController.isGhostActive)
            {
                //gameCam.transform.rotation = endRotation;
                gameCam.Follow = GameObject.Find("Ghost").transform;
            }
            else
            {
                //gameCam.transform.rotation = endRotation;
                gameCam.Follow = GameObject.Find("Player").transform;
            }
        }
    }
}
