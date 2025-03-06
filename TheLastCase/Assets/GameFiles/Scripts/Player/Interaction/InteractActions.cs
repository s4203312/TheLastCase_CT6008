using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class InteractActions : MonoBehaviour
{
    private IPuzzle puzzle;

    private GameObject interactionObject;
    public GameObject player;

    public InventoryItemData correctItem;

    [Header("Puzzle Managers")]
    public ManagerFiguresPuzzle figuresPuzzleManager;

    public void OpenDoor()
    {
        interactionObject = player.GetComponent<PlayerMovement>().interactedObject;

        interactionObject.SetActive(false);
        Debug.Log("Door Open");
    }

    public void PickUpItem()
    {
        interactionObject = player.GetComponent<PlayerMovement>().interactedObject;
        Button interactButton = interactionObject.GetComponent<InteractableObject>().playerButton;

        InventoryManager.Instance.AddItemToInventory(interactionObject.GetComponent<InteractableObject>().itemData);

        Destroy(interactionObject);
        interactButton.gameObject.SetActive(false);
    }

    public void PlaceItem()
    {
        interactionObject = player.GetComponent<PlayerMovement>().interactedObject;
        Button interactButton = interactionObject.GetComponent<InteractableObject>().playerButton;

        correctItem = interactionObject.GetComponent<CorrectItem>().correctItem;

        Transform puzzleItem = interactionObject.transform.parent.Find("PuzzleItem");
        Transform collider = interactionObject.transform.parent.Find("Collider");

        if (InventoryManager.IsItemInInventory(correctItem))
        {
            InventoryManager.Instance.RemoveItemFromInventory(correctItem);

            puzzleItem.gameObject.SetActive(true);
            collider.gameObject.SetActive(false);

            interactButton.gameObject.SetActive(false);
            correctItem = null;
        }

        else
        {
            // add dialogue of player saying you dont have anything for this yet
        }
    }
}
