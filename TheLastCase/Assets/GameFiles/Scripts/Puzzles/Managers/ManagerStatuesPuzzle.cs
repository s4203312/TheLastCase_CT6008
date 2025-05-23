using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ManagerStatuesPuzzle : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Name")]
    [SerializeField] private string puzzleID;

    private InventoryItemData itemData;
    //private bool iscorrect = false;
    private CinemachineBrain gameCam;
    [HideInInspector] public Transform hitSlot;
    private GameObject currentStatue = null;

    [Header("Used Objects")]
    public GameObject[] statues;
    public GameObject[] statueWeaponSilhouettes;
    public CinemachineVirtualCamera VirtualCamera;
    public InspectObject inpectManager;
    public GameObject inventoryPanel;
    public GameObject collisionBox;
    public Button placeItemButton;
    public Button pickUpItemButton;
    public Animator animator;
    public GameUI gameUI;

    private void Start()
    {
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);

        gameCam = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();

        foreach (GameObject weapons in statueWeaponSilhouettes) { weapons.SetActive(false); }
    }

    public void OnCollisionStay()
    {
        InspectingStatues();
        CheckPuzzle();
    }

    public void InspectingStatues()
    {
        foreach (GameObject statue in statues)
        {
            int modelIndex = Array.IndexOf(statues, statue);
            GameObject puzzleSlot = statue.transform.Find("PuzzleSlot").gameObject;
            PuzzleData puzzleData = puzzleSlot.GetComponent<PuzzleData>();

            gameCam.OutputCamera.transform.position = VirtualCamera.transform.position;
            gameCam.OutputCamera.transform.rotation = VirtualCamera.transform.rotation;

            Ray ray = new Ray(gameCam.OutputCamera.transform.position, gameCam.OutputCamera.transform.forward);
            RaycastHit hit;

            // Raycast, ignore "PuzzleSlot" layer
            if (Physics.Raycast(ray, out hit, 5f, ~LayerMask.GetMask("PuzzleSlot")) && hit.transform != null)
            {
                if (hit.transform.gameObject == statue)
                {
                    // Update current statue only if it changed
                    if (currentStatue != statue)
                    {
                        currentStatue = statue;
                    }
                }
            }

            // Only handle interaction for the current selected statue
            if (statue == currentStatue)
            {
                if (puzzleSlot.transform.childCount > 0)
                {
                    puzzleData.isOccupied = true;

                    GameObject heldItem = puzzleSlot.transform.GetChild(0).gameObject;
                    InteractableObject heldObject = heldItem.GetComponent<InteractableObject>();

                    float angle = Quaternion.Angle(statueWeaponSilhouettes[modelIndex].transform.rotation, heldItem.transform.rotation);

                    if (puzzleData.correctItem == heldObject.itemData && angle <= 12f)
                    {
                        // Lock weapon in place
                        heldItem.transform.rotation = statueWeaponSilhouettes[modelIndex].transform.rotation;
                        puzzleSlot.GetComponent<BoxCollider>().enabled = false;
                        statueWeaponSilhouettes[modelIndex].GetComponent<MeshRenderer>().enabled = false;

                        inpectManager.InspectionFunction(statue.transform.GetChild(0).transform.parent.gameObject, false);
                    }
                    else
                    {
                        // Still inspecting the item
                        inpectManager.InspectionFunction(heldItem, true);
                    }
                }
                else
                {
                    puzzleData.isOccupied = false;

                    // Inspect the statue normally
                    inpectManager.InspectionFunction(statue.transform.GetChild(0).transform.parent.gameObject, false);
                }

                HoveringOnSlot();
                statueWeaponSilhouettes[modelIndex].SetActive(true);
            }
            else
            {
                statueWeaponSilhouettes[modelIndex].SetActive(false);
            }          
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
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 10.0f);
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
    }


    public void CheckPuzzle()
    {
        int correctWeaponsPlaced = 0;

        foreach (GameObject statue in statues)
        {
            Transform puzzleSlot = statue.transform.Find("PuzzleSlot");
            PuzzleData puzzleData = puzzleSlot.GetComponent<PuzzleData>();
            int modelIndex = Array.IndexOf(statues, statue);

            if (puzzleSlot.childCount > 0)
            {
                puzzleData.isOccupied = true;

                InteractableObject placedObject = puzzleSlot.GetChild(0).GetComponent<InteractableObject>();
                bool correctItemPlaced = placedObject.itemData == puzzleData.correctItem;

                float angleDifference = Quaternion.Angle(
                    statueWeaponSilhouettes[modelIndex].transform.rotation,
                    puzzleSlot.GetChild(0).transform.rotation
                );

                bool isRotationCorrect = angleDifference <= 2f;

                // Only run this if the item is correct, rotation is good, and we haven't counted this one yet
                if (correctItemPlaced && isRotationCorrect && !puzzleData.isCorrectlyPlaced)
                {
                    puzzleSlot.GetChild(0).rotation = statueWeaponSilhouettes[modelIndex].transform.rotation;
                    //inpectManager.InspectionFunction(statue.transform.GetChild(0).transform.parent.gameObject, false);

                    puzzleSlot.GetComponent<BoxCollider>().enabled = false;
                    statueWeaponSilhouettes[modelIndex].GetComponent<MeshRenderer>().enabled = false;

                    puzzleData.isCorrectlyPlaced = true;
                }
                if (correctItemPlaced && placedObject.name == "wukong_Sword_Ground")
                {
                    placedObject.transform.rotation = statueWeaponSilhouettes[modelIndex].transform.rotation;
                }

                if (puzzleData.isCorrectlyPlaced)
                {
                    correctWeaponsPlaced++;
                    Debug.Log("Corect" + correctWeaponsPlaced);
                }
            }
            else
            {
                puzzleData.isOccupied = false;
            }
        }

        if (correctWeaponsPlaced == 3)
        {
            PuzzleComplete();
        }
    }

    public void PuzzleComplete()
    {
        foreach (GameObject statue in statues)
        {
            Transform collider = statue.transform.Find("Collider");
            collider.gameObject.SetActive(false);
        }
        collisionBox.SetActive(false);

        Debug.Log("Puzzle Complete");
        PuzzleRegistry.Instance.PuzzleFinished();
        GameObject.Find("Button_Interact").GetComponent<Button>().onClick.RemoveAllListeners();
        animator.SetTrigger("PuzzleComplete");
        gameUI.ExitView(1);
    }
}
