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
        //Only allowing the ghost to see the objects
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
            if (hitCollider.gameObject.TryGetComponent(out InteractableObject script))
            {
                if (script.onlyGhostVisable && !(hitCollider.gameObject.GetComponent<MeshRenderer>().enabled == true))
                {
                    hitCollider.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    loadedGhostObjects.Add(hitCollider);
                }
            }
        }
    }

    private void UpdateLoadedObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(ghost.transform.position, distanceOfOverlap);

        //Iterating backwards to ensure that a index error is not found when removing the item from list
        for (int i = 0; i < loadedGhostObjects.Count; i++)
        {
            Collider loadedObject = loadedGhostObjects[i];
            if (!hitColliders.ToList().Contains(loadedObject))
            {
                loadedGhostObjects.Remove(loadedObject);
                loadedObject.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    private void UnloadObjects()
    {
        foreach (var loadedObject in loadedGhostObjects)
        {
            loadedObject.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        loadedGhostObjects.Clear();
    }
}
