using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerFiguresPuzzle : MonoBehaviour, IPuzzle
{
    public InventoryItemData figure1;
    public InventoryItemData figure2;
    public InventoryItemData figure3;

    public GameObject[] figures;

    public void CheckPuzzle()
    {
        int correctFigureHeadsPlaced = 0;

        foreach (GameObject figure in figures)
        {
            if (figure.activeInHierarchy)
                correctFigureHeadsPlaced++;
        }

        if (correctFigureHeadsPlaced == 3)
        {
            Debug.Log("Puzzle Complete");
        }
    }

    public void PuzzleComplete()
    {

    }
}
