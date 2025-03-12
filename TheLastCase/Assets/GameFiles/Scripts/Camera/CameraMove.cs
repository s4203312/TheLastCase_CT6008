using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float transitionSpeed = 2.0f; // Speed of the camera movement

    private bool isMoving = false;

    // Call this when entering a room and pass the target position
    public void MoveCameraToRoom(Transform roomTarget)
    {
        if (!isMoving)
            StartCoroutine(MoveCameraCoroutine(roomTarget));
    }

    // Coroutine to smoothly move camera to target position
    private IEnumerator MoveCameraCoroutine(Transform target)
    {
        isMoving = true;

        // Get current position
        Vector3 startPosition = gameObject.GetComponent<CinemachineVirtualCamera>().transform.position;
        Vector3 endPosition = target.position;

        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            gameObject.GetComponent<CinemachineVirtualCamera>().transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        // Ensure final position is exact
        gameObject.GetComponent<CinemachineVirtualCamera>().transform.position = endPosition;
        isMoving = false;
    }
}
