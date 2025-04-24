using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ManagerStatuesPuzzle : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Name")]
    [SerializeField] private string puzzleID;

    private InventoryItemData itemData;
    private bool iscorrect = false;
    private CinemachineBrain gameCam;
    private int correctWeaponsPlaced = 0;
    [HideInInspector] public Transform hitSlot;

    [Header("Used Objects")]
    public GameObject[] statues;
    public GameObject[] statueWeaponSilhouettes;
    public CinemachineVirtualCamera VirtualCamera;
    public InspectObject inpectManager;
    public GameObject inventoryPanel;
    public Button placeItemButton;
    public Button pickUpItemButton;

    private void Start()
    {
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);

        gameCam = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();

        foreach (GameObject weapons in statueWeaponSilhouettes) { weapons.SetActive(false); }
    }

    public void Update()
    {
        foreach (GameObject statue in statues)
        {
            int modelIndex = Array.IndexOf(statues, statue);           
        
            GameObject currentStatue = null;

            gameCam.OutputCamera.transform.position = VirtualCamera.transform.position;
            gameCam.OutputCamera.transform.rotation = VirtualCamera.transform.rotation;

            int layerMask = (8);

            Ray ray = new Ray(gameCam.OutputCamera.transform.position, gameCam.OutputCamera.transform.forward);
            RaycastHit hit;        

            if (Physics.Raycast(ray, out hit, layerMask) && hit.transform.gameObject != null)
            {
                //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 10.0f);
                if (hit.transform.gameObject == statue)
                {
                    iscorrect = true;
                    currentStatue = statue;
                }
            }

            if (iscorrect && currentStatue != null)
            {
                GameObject puzzleSlot = statue.transform.Find("PuzzleSlot").gameObject;

                if (puzzleSlot.GetComponent<PuzzleData>().isOccupied == false)
                {
                    inpectManager.InspectionFunction(currentStatue.transform.GetChild(0).transform.parent.gameObject, false);
                }
                else
                {
                    inpectManager.InspectionFunction(puzzleSlot.transform.GetChild(0).gameObject, true);
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

        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("PuzzleSlot")) && !inventoryPanel.activeInHierarchy)
        {
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 10.0f);
            if (hit.transform.tag == "Silhouette")
            {

                hitSlot = hit.transform;
                ShowButtonInFrontOfSlot(hit.collider.gameObject);
            }
            else
            {
                placeItemButton.gameObject.SetActive(false);
                pickUpItemButton.gameObject.SetActive(false);
            }
        }
    }

    void ShowButtonInFrontOfSlot(GameObject hoveredSlot)
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
        activeButton.transform.position = hoveredSlot.transform.position + new Vector3(0f, 0f, 0f);
    }


    public void CheckPuzzle()
    {  
        foreach (GameObject statue in statues)
        {
            PuzzleData puzzleData = statue.transform.Find("PuzzleSlot").GetComponent<PuzzleData>();

            int modelIndex = Array.IndexOf(statues, statue);

            if (statue.transform.Find("PuzzleSlot").childCount > 0)
            {
                puzzleData.isOccupied = true;

                if (puzzleData.correctItem == puzzleData.transform.gameObject.transform.GetChild(0).GetComponent<InteractableObject>().itemData && Quaternion.Angle(statueWeaponSilhouettes[modelIndex].transform.rotation, puzzleData.transform.GetChild(0).transform.rotation) <= 12f)
                {                 
                    puzzleData.transform.GetChild(0).transform.rotation = statueWeaponSilhouettes[modelIndex].transform.rotation;
                    inpectManager.InspectionFunction(statue.transform.GetChild(0).transform.parent.gameObject, false);

                    puzzleData.transform.gameObject.GetComponent<BoxCollider>().enabled = false;

                    statueWeaponSilhouettes[modelIndex].GetComponent<MeshRenderer>().enabled = false;
                    correctWeaponsPlaced++;
                    Debug.Log(correctWeaponsPlaced);
                }
            }
            else
            {
                statue.transform.Find("PuzzleSlot").GetComponent<PuzzleData>().isOccupied = false;
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

        Debug.Log("Puzzle Complete");
    }
}
