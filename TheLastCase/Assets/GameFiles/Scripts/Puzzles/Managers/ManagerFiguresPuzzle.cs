using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerFiguresPuzzle : MonoBehaviour, IPuzzle
{
    public GameObject[] pedestals;

    private InventoryItemData itemData;

    public Animator animator;
    [SerializeField] GameObject key;

    [SerializeField] private string puzzleID;

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
            PuzzleData puzzleData = collider.GetComponent<PuzzleData>();

            if (puzzleData.itemHolder.childCount > 0)
            {
                itemData = puzzleData.itemHolder.GetChild(0).GetComponent<InteractableObject>().itemData; 
                puzzleData.isOccupied = true;
            }
            else
            {
                puzzleData.isOccupied = true;
            }

            if (puzzleData.correctItem == itemData && itemData != null)
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
            PuzzleData puzzleData = collider.GetComponent<PuzzleData>();

            collider.gameObject.SetActive(false);

            BoxCollider itemCollider =  puzzleData.itemHolder.GetChild(0).GetComponent<BoxCollider>();
            itemCollider.enabled = false;
        }

        key.SetActive(true);
        animator.SetTrigger("PuzzleComplete");

        Debug.Log("Puzzle Complete");
    }
}
