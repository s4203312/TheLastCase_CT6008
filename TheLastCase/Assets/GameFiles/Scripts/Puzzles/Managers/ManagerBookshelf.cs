using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBookshelf : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Name")]
    [SerializeField] private string puzzleID;

    private InventoryItemData itemData;

    [Header("Used Objects")]
    public BookshelfHover BookshelfHover;

    public GameObject[] shelfSlots;

    public GameObject bookshelfCam;
    public CinemachineVirtualCamera VirtualCamera;

    void Start()
    {
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);
        BookshelfHover.enabled = false;
    }

    void Update()
    {
        if (bookshelfCam.transform.position == VirtualCamera.transform.position)
        {
            BookshelfHover.enabled = true;
        }
        else
        {
            BookshelfHover.placeItemButton.gameObject.SetActive(false);
            BookshelfHover.enabled = false;
        }
    }

    public void CheckPuzzle()
    {
        int correctBooksPlaced = 0;

        foreach (GameObject slot in  shelfSlots)
        {
            PuzzleSlotData puzzleSlotData = slot.GetComponent<PuzzleSlotData>();

            if (puzzleSlotData.itemHolder.childCount > 0)
            {
                itemData = slot.transform.GetChild(0).GetComponent<InteractableObject>().itemData;
                puzzleSlotData.isOccupied = true;
            }
            else
            {
                puzzleSlotData.isOccupied = false;
            }

            if (puzzleSlotData.correctItem == itemData && itemData != null)
            {
                correctBooksPlaced++;
            }
        }
    }

    public void PuzzleComplete()
    {

    }
}
