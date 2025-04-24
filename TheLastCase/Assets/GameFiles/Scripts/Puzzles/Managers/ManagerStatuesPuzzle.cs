using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerStatuesPuzzle : MonoBehaviour, IPuzzle
{
    [Header("Puzzle Name")]
    [SerializeField] private string puzzleID;

    private InventoryItemData itemData;
    private bool iscorrect = false;

    [Header("Used Objects")]
    public GameObject[] statues;
    public GameObject[] statueWeaponSilhouettes;
    public CinemachineVirtualCamera VirtualCamera;
    public InspectObject inpectManager;

    private void Start()
    {
        PuzzleRegistry.Instance.RegisterPuzzle(puzzleID, this);

        foreach (GameObject weapons in statueWeaponSilhouettes) { weapons.SetActive(false); }
    }

    public void Update()
    {
        foreach (GameObject statue in statues)
        {
            int modelIndex = System.Array.IndexOf(statues, statue);

            if (statue.transform.GetChild(0).transform.position == VirtualCamera.transform.position) { iscorrect = true; }

            if (iscorrect)
            {
                inpectManager.InspectionFunction(statue.transform.GetChild(0).transform.parent.gameObject, false);
                
            }
        }
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
