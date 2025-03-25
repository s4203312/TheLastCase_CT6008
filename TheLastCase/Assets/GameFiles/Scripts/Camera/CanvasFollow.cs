using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject ghost;
    [SerializeField] private bool Camera;


    private Vector3 cameraPerspective;
    private Vector3 canvasPerspective;

    public PlayerController playerController;
    private bool isGhostActive;

    private void Start()
    {
        cameraPerspective = new Vector3(3f, 12.5f, 0.2f);
        canvasPerspective = new Vector3(0.0f, 3f, 0.0f);
    }

    private void Update()
    {
        isGhostActive = playerController.isGhostActive;

        if (isGhostActive)
        {
            if (Camera)
            {
                transform.position = ghost.transform.position + cameraPerspective;
            }
            else
            {
                transform.position = ghost.transform.position + canvasPerspective;
            }
        }
        else
        {
            if (Camera)
            {
                transform.position = player.transform.position + cameraPerspective;
            }
            else
            {
                transform.position = player.transform.position + canvasPerspective;
            }
        }
    }
}
