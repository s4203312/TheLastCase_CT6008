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
    [SerializeField] GameObject key;

    private void Start()
    {
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);
    }

    public void CheckPuzzle()
    {
        int correctFigureHeadsPlaced = 0;

        foreach (GameObject pedestal in pedestals)
        {
            Transform collider = pedestal.transform.Find("Collider");
            PuzzleSlotData puzzleSlotData = collider.GetComponent<PuzzleSlotData>();

            if (puzzleSlotData.itemHolder.childCount > 0)
            {
                if (puzzleSlotData.isOccupied == false)
                {
                    itemData = puzzleSlotData.itemHolder.GetChild(0).GetComponent<InteractableObject>().itemData;
                    puzzleSlotData.isOccupied = true;
                }                             
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

        key.SetActive(true);
        animator.SetTrigger("PuzzleComplete");

        Debug.Log("Puzzle Complete");
    }
}
