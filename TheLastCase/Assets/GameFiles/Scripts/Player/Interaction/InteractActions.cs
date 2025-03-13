using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractActions : MonoBehaviour
{
    private IPuzzle currentPuzzle;

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
        interactionObject = player.GetComponent<PlayerMovement>().interactedObject;
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
        interactionObject = player.GetComponent<PlayerMovement>().interactedObject;

        InventoryManager.Instance.AddItemToInventory(interactionObject.GetComponent<InteractableObject>().itemData);

        Destroy(interactionObject);
        interactButton.gameObject.SetActive(false);
    }

    public void PlaceItem(string itemPosition)
    {
        if (Time.time - lastClickTime < cooldownTime)
        {
            return; // Exit the function if the cooldown hasn't passed
        }

        lastClickTime = Time.time;

        interactionObject = player.GetComponent<PlayerMovement>().interactedObject;
        Transform itemTransform = interactionObject.transform.parent.Find("PuzzleItem").transform;

        int index = int.Parse(itemPosition) - 1;

        if (index >= 0 && index < InventoryManager.Instance.Inventory.Count)
        {
            InventoryItemData itemData = InventoryManager.Instance.Inventory[index];

            GameObject chosenItem = itemData.itemObject;

            if (chosenItem != null)
            {
                GameObject item = Instantiate(chosenItem, itemTransform.position, Quaternion.identity);
                //item.transform.SetParent(itemTransform.transform);
                item.SetActive(true);

                Debug.Log("Item Placed");

                InventoryManager.Instance.RemoveItemFromInventory(itemData);
            }
        }
    }

    public void AccessInventory()
    {
        Transform inventoryManager = Managers.transform.Find("InventoryManager");

        inventoryManager.gameObject.GetComponent<UIInventoryLoad>().LoadInventory();

    }


    //Ghost Actions

    public void KeyholeSquish()
    {
        interactionObject = player.GetComponent<PlayerMovement>().interactedObject;

        //ghost.transform.position = interactionObject.transform.position + new Vector3(-2,0,0);

        //Doesnt work yet issues with door?
    }
}
