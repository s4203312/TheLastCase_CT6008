using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostObjectsAppear : MonoBehaviour
{
    public PlayerController playerController;
    private GameObject ghost;

    public List<Collider> loadedGhostObjects = new List<Collider>();

    private int distanceOfOverlap = 10;

    private void Start()
    {
        ghost = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if (playerController.isGhostActive) 
        { 
            LoadObjects();
            UpdateLoadedObjects();
        }
        else
        {
            UnloadObjects();
        }
    }

    private void LoadObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(ghost.transform.position, distanceOfOverlap);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Interactable"))
            {
                Debug.Log("Interactable" + hitCollider);
                if (hitCollider.gameObject.TryGetComponent<InteractableObject>(out InteractableObject script))
                {
                    if (script.onlyGhostVisable && !(hitCollider.gameObject.GetComponent<MeshRenderer>().enabled == true))
                    {
                        Debug.Log(hitCollider);
                        hitCollider.gameObject.GetComponent<MeshRenderer>().enabled = true;
                        //hitCollider.gameObject.GetComponent<Collider>().enabled = true;
                        loadedGhostObjects.Add(hitCollider);
                    }
                }
            }
        }
    }

    private void UpdateLoadedObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(ghost.transform.position, distanceOfOverlap);
        //Iterating bakcwards to ensure that a index error is not found when removing the item from list
        for (int i = 0; i < loadedGhostObjects.Count; i++)
        {
            Collider loadedObject = loadedGhostObjects[i];
            if (!hitColliders.ToList().Contains(loadedObject))
            {
                loadedGhostObjects.Remove(loadedObject);
                loadedObject.gameObject.GetComponent<MeshRenderer>().enabled = false;
                //loadedObject.gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    private void UnloadObjects()
    {
        foreach (var loadedObject in loadedGhostObjects)
        {
            loadedObject.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //loadedObject.gameObject.GetComponent<Collider>().enabled = false;
        }
        loadedGhostObjects.Clear();
    }
}
