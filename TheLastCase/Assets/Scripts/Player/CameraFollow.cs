using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 perspective;

    private void Start()
    {
        perspective = new Vector3(3f, 12.5f, 0.2f);
    }

    private void Update()
    {
        transform.position = player.transform.position + perspective;
    }
}
