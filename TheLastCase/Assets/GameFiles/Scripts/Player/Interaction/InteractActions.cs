using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
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
        //interactionObject = player.GetComponent<PlayerMovement>().interactedObject;

        //Transform puzzleItem = interactionObject.transform.parent.Find("PuzzleItem");
        //Transform collider = interactionObject.transform.parent.Find("Collider");

        //InventoryManager.Instance.Inventory[int.Parse(itemPosition)]

        //if (InventoryManager.IsItemInInventory(correctItem))
        //{
        //    InventoryManager.Instance.RemoveItemFromInventory(correctItem);

        //    puzzleItem.gameObject.SetActive(true);
        //    collider.gameObject.SetActive(false);

        //    interactButton.gameObject.SetActive(false);
        //    correctItem = null;
        //}

        //else
        //{
        // add dialogue of player saying you dont have anything for this yet
        //}
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
