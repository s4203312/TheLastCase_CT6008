using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class InteractActions : MonoBehaviour
{

    private GameObject interactionObject;
    public GameObject player;
    public GameObject ghost;
    public Button interactButton;

    public GameObject Managers;

    private float cooldownTime = 0.5f;
    private float lastClickTime = 0f;

    //Player Actions

    public void OpenDoor()
    {
        interactionObject = GetComponent<PlayerMovement>().interactedObject;
        GameObject pivotPoint = interactionObject.transform.GetChild(0).gameObject;

        interactionObject.GetComponent<BoxCollider>().enabled = false;
        interactButton.gameObject.SetActive(false);

        StartCoroutine(OpeningDoor(pivotPoint));
    }

    public IEnumerator OpeningDoor(GameObject pivotPoint)
    {
        float targetAngle = Random.Range(75,105);
        float rotatedAngle = 0f;
        float rotationSpeed = 45f;

        while (rotatedAngle < targetAngle)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            interactionObject.transform.RotateAround(pivotPoint.transform.position, Vector3.down, rotationStep);
            rotatedAngle += rotationStep;
            yield return null;
        }
    }

    public void PickUpItem()
    {
        interactionObject = GetComponent<PlayerMovement>().interactedObject;

        InventoryManager.Instance.AddItemToInventory(interactionObject.GetComponent<InteractableObject>().itemData, interactionObject);
        Transform inventoryManager = Managers.transform.Find("InventoryManager");
        inventoryManager.gameObject.GetComponent<UIInventoryLoad>().InventoryClose();

        interactionObject.SetActive(false);
        interactButton.gameObject.SetActive(false);
    }

    public void PlaceItem(string itemPosition)
    {
        if (Time.time - lastClickTime < cooldownTime)
        {
            return;
        }

        lastClickTime = Time.time;

        interactionObject = GetComponent<PlayerMovement>().interactedObject;
        Transform itemTransform = interactionObject.transform.parent.Find("PuzzleSlot").transform;
        Transform inventoryManager = Managers.transform.Find("InventoryManager");

        int index = int.Parse(itemPosition) - 1;

        if (index >= 0 && index < InventoryManager.Instance.Inventory.Count)
        {
            InventoryItemData itemData = InventoryManager.Instance.Inventory[index];
            GameObject itemObject = InventoryManager.Instance.InventoryGameObjects[index];
            Debug.Log(itemObject);

            if (itemObject != null)
            {
                itemObject.transform.position = itemTransform.position;
                itemObject.transform.SetParent(itemTransform);
                itemObject.SetActive(true);

                PuzzleRegistry.Instance.CheckPuzzleByID(itemData.puzzleID);

                InventoryManager.Instance.RemoveItemFromInventory(itemData, itemObject);
                inventoryManager.gameObject.GetComponent<UIInventoryLoad>().InventoryClose();
                interactButton.gameObject.SetActive(false);
            }
        }
    }

    public void InspectPuzzle()
    {

    }

    public void AccessInventory()
    {
        Transform inventoryManager = Managers.transform.Find("InventoryManager");

        inventoryManager.gameObject.GetComponent<UIInventoryLoad>().LoadInventory(false);

    }


    //Ghost Actions

    public void KeyholeSquish()
    {
        interactionObject = GetComponent<PlayerMovement>().interactedObject;

        ghost.GetComponent<NavMeshAgent>().Warp(ghost.transform.position + (-interactionObject.transform.right * 2));

        //Doesnt work yet issues with door?
    }
}
