using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerShelfPuzzle : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Name")]
    [SerializeField] private string puzzleID;

    private InventoryItemData itemData;

    [Header("Used Objects")]
    public ShelfHover shelfHover;

    public GameObject[] shelfSlots;
    public Animator animator;
    public GameObject shelfCam;
    public CinemachineVirtualCamera VirtualCamera;

    void Start()
    {
        shelfHover.slots = shelfSlots;
        shelfHover.placeItemButton.gameObject.SetActive(false);
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);
        shelfHover.enabled = false;
    }

    void Update()
    {
        if (shelfCam.transform.position == VirtualCamera.transform.position)
        {
            shelfHover.enabled = true;
        }
        else
        {
            shelfHover.placeItemButton.gameObject.SetActive(false);
            shelfHover.enabled = false;
        }
    }

    public void CheckPuzzle()
    {
        int correctBooksPlaced = 0;

        foreach (GameObject slot in  shelfSlots)
        {
            PuzzleData puzzleSlotData = slot.GetComponent<PuzzleData>();

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

        if (correctBooksPlaced == shelfSlots.Length) 
        {
            PuzzleComplete();
        }
    }

    public void PuzzleComplete()
    {
        foreach (GameObject slot in shelfSlots) { slot.GetComponent<BoxCollider>().enabled = false; }

        shelfHover.gameObject.transform.GetChild(1).transform.gameObject.SetActive(false);

        animator.SetTrigger("PuzzleComplete");
        GameObject.Find("GameUI").GetComponent<GameUI>().ExitView(1);

        PuzzleRegistry.Instance.PuzzleFinished();
    }
}
