using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class InteractActions : MonoBehaviour
{
    private GameObject interactionObject;
    public GameObject player;

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

        if (InventoryManager.IsItemInInventory(figuresPuzzleManager.correctItem))
        {
            InventoryManager.Instance.RemoveItemFromInventory(figuresPuzzleManager.correctItem);

            interactionObject.transform.parent.GetChild(0).gameObject.SetActive(true);
            figuresPuzzleManager.correctFigureHeadsPlaced++;
            interactButton.gameObject.SetActive(false);
        }

        else
        {
            // add dialogue of player saying you dont have anything for this yet
        }
    }
}
