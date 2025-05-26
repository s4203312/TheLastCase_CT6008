using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ManagerPosessionBallPuzzle : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Name")]
    [SerializeField] private string puzzleID;

    [Header("Posession Objects")]
    public GameObject movingObject;
    public GameObject movingObjectSlot;

    [Header("Used Objects")]
    public GameObject tableCam;
    public CinemachineVirtualCamera VirtualCamera;
    public GameObject inventoryPanel;
    public BoxCollider puzzleItem;
    public GameObject puzzleSlot;
    public Animator animatorBook;
    public Animator animatorBall;
    public GameUI gameUI;
    public Button placeItemButton;
    public Button pickUpItemButton;

    private bool isItemInCorrectPos;
    private CinemachineBrain gameCam;
    [HideInInspector] public Transform hitSlot;
    private bool isBallPlaced = false;

    private void Start()
    {
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);
        puzzleItem.enabled = false;
        movingObjectSlot.SetActive(false);

        gameCam = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
    }

    private void Update()
    {
        if (tableCam.transform.position == VirtualCamera.transform.position)
        {
            if (!isBallPlaced) { HoveringOnSlot(); }

            if (!isItemInCorrectPos && isBallPlaced) 
            { 
                movingObject.GetComponent<GhostPosession>().enabled = true;
            }          
        }
        else
        {
            movingObject.GetComponent<GhostPosession>().enabled = false;
        }
    }

    public void HoveringOnSlot()
    {
        gameCam.OutputCamera.transform.position = VirtualCamera.transform.position;
        gameCam.OutputCamera.transform.rotation = VirtualCamera.transform.rotation;

        Ray ray = gameCam.OutputCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f, LayerMask.GetMask("PuzzleSlot")) && !inventoryPanel.activeInHierarchy)
        {
            if (hit.transform.tag == "Silhouette")
            {

                hitSlot = hit.transform;
                ShowButtonInFrontOfSlot(hit.collider.gameObject, hit.point);
            }
            else
            {
                placeItemButton.gameObject.SetActive(false);
                pickUpItemButton.gameObject.SetActive(false);
            }
        }
        else
        {
            placeItemButton.gameObject.SetActive(false);
            pickUpItemButton.gameObject.SetActive(false);
            hitSlot = null;
        }
    }

    void ShowButtonInFrontOfSlot(GameObject hoveredSlot, Vector3 hitPoint)
    {
        GameObject activeButton = null;
        if (hitSlot.childCount > 0 || hitSlot.GetComponent<PuzzleData>().isOccupied)
        {
            activeButton = pickUpItemButton.gameObject;
        }
        else
        {
            activeButton = placeItemButton.gameObject;
        }
        activeButton.gameObject.SetActive(true);
        activeButton.transform.position = hitPoint;
        activeButton.transform.rotation = Quaternion.LookRotation(gameCam.OutputCamera.transform.forward);
    }

    public void CheckPuzzle()
    {
        if (puzzleSlot.transform.childCount > 0)
        {
            if (puzzleSlot.transform.GetChild(0).GetComponent<InteractableObject>().itemData == puzzleSlot.GetComponent<PuzzleData>().correctItem)
            {
                puzzleSlot.GetComponent<BoxCollider>().enabled = false;
                puzzleSlot.GetComponent<MeshRenderer>().enabled = false;

                GameObject ball = GameObject.Find("yogaBall_Possess");

                Destroy(ball.GetComponent<Animator>());
                ball.tag = "Posessable";             
                isBallPlaced = true;

                movingObjectSlot.SetActive(true);
                animatorBall.SetTrigger("ShowPath");

            }
        }

        if (Vector3.Distance(movingObject.transform.position, movingObjectSlot.transform.position) < 0.2f)
        {
            if (movingObject.GetComponent<InteractableObject>().itemData == movingObjectSlot.GetComponent<PuzzleData>().correctItem)
            {
                movingObject.transform.position = movingObjectSlot.transform.position;
                isItemInCorrectPos = true;
                movingObject.GetComponent<GhostPosession>().enabled = false;

                movingObjectSlot.SetActive(false);
                PuzzleComplete();
            }         
        }
    }

    public void PuzzleComplete()
    {
        tableCam.transform.parent.Find("Collider").transform.gameObject.SetActive(false);
        puzzleItem.enabled = true;
        PuzzleRegistry.Instance.PuzzleFinished();
        GameObject.Find("Button_Interact").GetComponent<Button>().onClick.RemoveAllListeners();
        pickUpItemButton.onClick.RemoveAllListeners();
        placeItemButton.onClick.RemoveAllListeners();
        animatorBook.SetTrigger("PuzzleComplete");
        gameUI.ExitView(1);
    }
}
