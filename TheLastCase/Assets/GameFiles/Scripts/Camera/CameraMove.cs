using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private PlayerController playerController;
    private CinemachineVirtualCamera gameCam;
    public float transitionSpeed = 2.0f; //Speed of the camera movement
    private bool isMoving = false;

    private void Start()
    {
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        gameCam = GetComponent<CinemachineVirtualCamera>();
        gameCam.Follow = null;

        //Checking when object to follow
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        if (gameCam.Follow == null)
        {
            if (playerController.isGhostActive)
            {
                gameCam.Follow = GameObject.Find("Ghost").transform;
                gameCam.LookAt = GameObject.Find("Ghost").transform;
            }
            else
            {
                gameCam.Follow = GameObject.Find("Player").transform;
                gameCam.LookAt = GameObject.Find("Player").transform;
            }
        }
    }

    // Call this when entering a room and pass the target position
    public void MoveCameraToRoom(Vector3 pos, Quaternion rot)
    {
        if (!isMoving)
            StartCoroutine(MoveCameraCoroutine(pos, rot));
    }

    // Coroutine to smoothly move camera to target position
    private IEnumerator MoveCameraCoroutine(Vector3 roomPos, Quaternion roomRot)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameCam.Follow = null;
        gameCam.LookAt = null;

        isMoving = true;

        // Get current position
        Vector3 startPosition = gameObject.GetComponent<CinemachineVirtualCamera>().transform.position;
        Vector3 endPosition = roomPos;
        Quaternion startRotation = gameObject.GetComponent<CinemachineVirtualCamera>().transform.rotation;
        Quaternion endRotation = roomRot;

        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            gameObject.GetComponent<CinemachineVirtualCamera>().transform.position = Vector3.Lerp(startPosition, endPosition, t);
            gameObject.GetComponent<CinemachineVirtualCamera>().transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        // Ensure final position is exact
        gameObject.GetComponent<CinemachineVirtualCamera>().transform.position = endPosition;       

        isMoving = false;

        // Re-enable mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
