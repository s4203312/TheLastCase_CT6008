using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject puzzleItem;
    public Animator animator;
    public GameUI gameUI;

    private bool isItemInCorrectPos;

    private void Start()
    {
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);
        puzzleItem.SetActive(false);
        movingObjectSlot.SetActive(false);

    }

    private void Update()
    {
        if (tableCam.transform.position == VirtualCamera.transform.position)
        {
            if (!isItemInCorrectPos) 
            { 
                movingObject.GetComponent<GhostPosession>().enabled = true;

                movingObjectSlot.SetActive(true);
            }          
        }
        else
        {
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
        tableCam.transform.parent.Find("Collider").transform.gameObject.SetActive(false);
        puzzleItem.SetActive(true);
        Debug.Log("Puzzle Complete");
        PuzzleRegistry.Instance.PuzzleFinished();
        animator.SetTrigger("PuzzleComplete");
        gameUI.ExitView(1);
    }
}
