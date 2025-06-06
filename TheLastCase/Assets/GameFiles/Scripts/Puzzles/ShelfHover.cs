using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ShelfHover : MonoBehaviour             //Original logic for hovering over slot.
{  
    public Button placeItemButton;
    public Button pickUpItemButton;

    private CinemachineBrain gameCam;
    private CinemachineVirtualCamera virtualCam;
    public GameObject inventoryPanel;

    [HideInInspector]
    public GameObject[] slots;
    [HideInInspector]
    public Transform hitSlot;

    void Awake()
    {
        virtualCam = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        gameCam = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();

        gameCam.OutputCamera.transform.position = virtualCam.transform.position;
        gameCam.OutputCamera.transform.rotation = virtualCam.transform.rotation;

        Ray ray = gameCam.OutputCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("PuzzleSlot")) && !inventoryPanel.activeInHierarchy)
        {
            if (Array.Exists(slots, slot => slot == hit.collider.gameObject))
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
        else
        {
            placeItemButton.gameObject.SetActive(false);
            pickUpItemButton.gameObject.SetActive(false);
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
        activeButton.transform.position = hoveredSlot.transform.position + new Vector3(0f, .5f, 2f);
    }
}