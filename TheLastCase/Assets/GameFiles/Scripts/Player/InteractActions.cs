using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InteractActions : MonoBehaviour
{
    private GameObject interactionObject;
    public GameObject player; 

    public void OpenDoor()
    {
        interactionObject = player.GetComponent<PlayerMovement>().interactedObject;

        interactionObject.SetActive(false);
        Debug.Log("Door Open");
    }
}
