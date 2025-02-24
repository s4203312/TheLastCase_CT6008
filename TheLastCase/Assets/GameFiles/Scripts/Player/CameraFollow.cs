using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private bool Camera;

    private Vector3 cameraPerspective;
    private Vector3 canvasPerspective;

    private void Start()
    {
        cameraPerspective = new Vector3(3f, 12.5f, 0.2f);
        canvasPerspective = new Vector3(0.0f, 3f, 0.0f);
    }

    private void Update()
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
