using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class InspectObject : MonoBehaviour
{
    public UIInventoryLoad inventoryLoadScipt;

    private float dRotationX;
    private float dRotationY;
    private float currentX;
    private float currentY;
    private float rotationSpeed = 4;

    private CinemachineVirtualCamera virtualCam;

    private void Start()
    {
        inventoryLoadScipt = GameObject.Find("InventoryManager").GetComponent<UIInventoryLoad>();
        virtualCam = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (inventoryLoadScipt.isCurrentlyInspecting)
        {
            GameObject inspectingObject = inventoryLoadScipt.currentlyInspectingObject;

            InspectionFunction(inspectingObject, true);
        }
    }

    public void InspectionFunction(GameObject inspectingObject, bool smallObject)
    {
        dRotationX = -Input.GetAxis("Mouse X");
        dRotationY = -Input.GetAxis("Mouse Y");

        //Perform rotation of object with mouse
        if (Input.GetMouseButton(1) && inspectingObject != null)
        {
            if (smallObject)
            {
                inspectingObject.transform.rotation = Quaternion.AngleAxis(dRotationX * rotationSpeed, transform.up) *  //Rotates on X axis
                Quaternion.AngleAxis(dRotationY * rotationSpeed, transform.right) *                                 //Rotates on Y axis
                inspectingObject.transform.rotation;                                            //Multiplying the original rotation
            }
            else
            {
                virtualCam.transform.RotateAround(inspectingObject.transform.position, Vector3.up, (dRotationX * (rotationSpeed * 2f)));
                virtualCam.LookAt = inspectingObject.transform;
            }
                                                                         
        }

        //Interacts with object if possible
        if (Input.GetMouseButtonDown(0) && inspectingObject != null)
        {
            var gameCam = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();

            gameCam.OutputCamera.transform.position = virtualCam.transform.position;
            gameCam.OutputCamera.transform.rotation = virtualCam.transform.rotation;

            Ray ray = gameCam.OutputCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.tag == "InspectInteractable")
                {
                    Debug.Log("Play animation here");
                }
            }
        }
    }
}
