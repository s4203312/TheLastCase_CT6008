using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ManagerPosessionSimplePuzzle : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Name")]
    [SerializeField] private string puzzleID;

    [Header("Posession Objects")]
    public GameObject movingObject;
    public GameObject movingObjectSlot;

    [Header("Used Objects")]
    public GameObject tableCam;
    public CinemachineVirtualCamera VirtualCamera;

    private bool isItemInCorrectPos;

    private void Start()
    {
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);

        movingObjectSlot.SetActive(false);
    }

    private void Update()
    {
        if (tableCam.transform.position == VirtualCamera.transform.position)
        {
            //foreach (GameObject movingObject in movingObjects)
            //{
            //    movingObject.GetComponent<GhostPosession>().enabled = true;
            //}
            if (!isItemInCorrectPos) 
            { 
                movingObject.GetComponent<GhostPosession>().enabled = true;

                movingObjectSlot.SetActive(true);
            }          
        }
        else
        {
            //foreach (GameObject movingObject in movingObjects)
            //{
            //    movingObject.GetComponent<GhostPosession>().enabled = false;
            //}
            movingObject.GetComponent<GhostPosession>().enabled = false;

            movingObjectSlot.SetActive(false);
        }
    }

    public void CheckPuzzle()
    {
        if (Vector3.Distance(movingObject.transform.position, movingObjectSlot.transform.position) < 0.2f)
        {
            if (movingObject.GetComponent<InteractableObject>().itemData == movingObjectSlot.GetComponent<PuzzleData>().correctItem)
            {
                movingObject.transform.position = movingObjectSlot.transform.position;
                isItemInCorrectPos = true;
                movingObject.GetComponent<GhostPosession>().enabled = false;

                PuzzleComplete();
            }         
        }
    }

    public void PuzzleComplete()
    {
        Debug.Log("Puzzle Complete");
    }
}
