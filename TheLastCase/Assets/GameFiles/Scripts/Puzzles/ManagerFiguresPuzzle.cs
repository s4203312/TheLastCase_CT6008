using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerFiguresPuzzle : MonoBehaviour
{
    public int correctFigureHeadsPlaced = 0;

    void Update()
    {
        if (correctFigureHeadsPlaced == 3)
        {
            Debug.Log("Puzzle Complete");
        }
    }
}
