using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookshelfHover : MonoBehaviour
{
    public GameObject[] slots;   
    public Button placeItemButton;

    private CinemachineBrain gameCam;
    private CinemachineVirtualCamera virtualCam;

    void Awake()
    {
        virtualCam = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        placeItemButton.gameObject.SetActive(false);
    }

    void Update()
    {
        gameCam = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();

        gameCam.OutputCamera.transform.position = virtualCam.transform.position;
        gameCam.OutputCamera.transform.rotation = virtualCam.transform.rotation;

        Ray ray = gameCam.OutputCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (Array.Exists(slots, slot => slot == hit.collider.gameObject))
            {
                ShowButtonInFrontOfSlot(hit.collider.gameObject);
            }
            else
            {
                placeItemButton.gameObject.SetActive(false);
            }
        }
    }


    void ShowButtonInFrontOfSlot(GameObject hoveredSlot)
    {
        placeItemButton.gameObject.SetActive(true);
        placeItemButton.transform.position = hoveredSlot.transform.position + new Vector3(0f, 0.5f, 0f);
    }
}