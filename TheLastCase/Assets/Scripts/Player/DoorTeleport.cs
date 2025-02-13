using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    [SerializeField] private Transform outPos;
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        player.transform.position = outPos.position;
    }
}
