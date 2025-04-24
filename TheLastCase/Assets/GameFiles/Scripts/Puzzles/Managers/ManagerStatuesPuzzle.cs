using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerStatuesPuzzle : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Name")]
    [SerializeField] private string puzzleID;

    private InventoryItemData itemData;

    [Header("Used Objects")]
    public GameObject[] statues;
    public GameObject[] statueWeaponSilhouettes;

    private void Start()
    {
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);

        foreach (GameObject weapons in statueWeaponSilhouettes) { weapons.SetActive(false); }
    }

    public void CheckPuzzle()
    {
        int correctFigureHeadsPlaced = 0;

        foreach (GameObject statue in statues)
        {
            
        }

        if (correctFigureHeadsPlaced == 3)
        {
            PuzzleComplete();
        }
    }

    public void PuzzleComplete()
    {
        foreach (GameObject statue in statues)
        {
            Transform collider = statue.transform.Find("Collider");
            PuzzleData puzzleSlotData = collider.GetComponent<PuzzleData>();

            collider.gameObject.SetActive(false);

            BoxCollider itemCollider = puzzleSlotData.itemHolder.GetChild(0).GetComponent<BoxCollider>();
            itemCollider.enabled = false;
        }

        Debug.Log("Puzzle Complete");
    }
}
