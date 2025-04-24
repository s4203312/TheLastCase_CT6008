using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject sparkleEffect;

    private void Start()
    {
        InteractableObject[] interactableObjects = FindObjectsOfType<InteractableObject>();

        foreach (InteractableObject interactableObject in interactableObjects)
        {
            GameObject interObj = interactableObject.gameObject;

            if (interObj.GetComponent<InteractableObject>().itemData != null )
            {
                Instantiate(sparkleEffect, interObj.transform);
            }
        }
    }
}
