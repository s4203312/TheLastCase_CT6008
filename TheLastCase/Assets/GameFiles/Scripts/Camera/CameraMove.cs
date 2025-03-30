using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float transitionSpeed = 2.0f; // Speed of the camera movement

    private bool isMoving = false;

    // Call this when entering a room and pass the target position
    public void MoveCameraToRoom(Vector3 roomPos, Quaternion roomRot)
    {
        if (!isMoving)
            StartCoroutine(MoveCameraCoroutine(roomPos, roomRot));
    }

    // Coroutine to smoothly move camera to target position
    private IEnumerator MoveCameraCoroutine(Vector3 roomPos, Quaternion roomRot)
    {
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
    }
}
