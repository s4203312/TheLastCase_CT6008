using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerFiguresPuzzle : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Name")]
    [SerializeField] private string puzzleID;

    private InventoryItemData itemData;

    [Header("Used Objects")]
    public GameObject[] pedestals;
    public Animator animator;
    [SerializeField] GameObject note;

    private void Start()
    {
        note.SetActive(false);
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);
    }

    public void CheckPuzzle()
    {
        int correctFigureHeadsPlaced = 0;

        foreach (GameObject pedestal in pedestals)
        {
            Transform collider = pedestal.transform.Find("Collider");
            PuzzleSlotData puzzleSlotData = collider.GetComponent<PuzzleSlotData>();

            InventoryItemData itemData = null;

            if (puzzleSlotData.itemHolder.childCount > 0)
            {
                itemData = puzzleSlotData.itemHolder.GetChild(0).GetComponent<InteractableObject>().itemData;
                puzzleSlotData.isOccupied = true;

            }
            else
            {
                puzzleSlotData.isOccupied = false;
            }

            if (puzzleSlotData.correctItem == itemData && itemData != null)
            {
                correctFigureHeadsPlaced++;
            }
        }

        if (correctFigureHeadsPlaced == 3)
        {        
            PuzzleComplete();          
        }
    }

    public void PuzzleComplete()
    {
        foreach (GameObject pedestal in pedestals)
        {
            Transform collider = pedestal.transform.Find("Collider");
            PuzzleSlotData puzzleSlotData = collider.GetComponent<PuzzleSlotData>();

            collider.gameObject.SetActive(false);

            BoxCollider itemCollider = puzzleSlotData.itemHolder.GetChild(0).GetComponent<BoxCollider>();
            itemCollider.enabled = false;
        }

        note.SetActive(true);
        animator.SetTrigger("PuzzleComplete");

        Debug.Log("Puzzle Complete");
    }
}
