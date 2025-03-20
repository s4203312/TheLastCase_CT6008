using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Button exitView;
    private Vector3 oldCameraPos;
    private Quaternion oldCameraRot;

    private void Start()
    {
        exitView.gameObject.SetActive(false);
    }

    public void ExitView()
    {
        GameObject playerCharacters = GameObject.Find("PlayerCharacters");
        CinemachineVirtualCamera virtCam = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        CameraMove cameraMove = virtCam.GetComponent<CameraMove>();
        oldCameraPos = playerCharacters.GetComponent<InteractActions>().oldCameraPos;
        oldCameraRot = playerCharacters.GetComponent<InteractActions>().oldCameraRot;


        if (cameraMove != null)
        {
            cameraMove.MoveCameraToRoom(oldCameraPos, oldCameraRot);

            playerCharacters.GetComponent<PlayerMovement>().enabled = true;

            exitView.gameObject.SetActive(false);
        }
    }
}
