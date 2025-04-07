using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectObject : MonoBehaviour
{
    public UIInventoryLoad inventoryLoadScipt;
    private GameObject inspectingObject;

    private float dRotationX;
    private float dRotationY;
    private float rotationSpeed = 4;

    private void Start()
    {
        inventoryLoadScipt = GameObject.Find("InventoryManager").GetComponent<UIInventoryLoad>();
    }

    private void Update()
    {
        if (inventoryLoadScipt.isCurrentlyInspecting)
        {
            inspectingObject = inventoryLoadScipt.currentlyInspectingObject;
            dRotationX = -Input.GetAxis("Mouse X");
            dRotationY = -Input.GetAxis("Mouse Y");

            //Perform rotation of object with mouse
            if (Input.GetMouseButton(1) && inspectingObject != null)
            {
                inspectingObject.transform.rotation = Quaternion.AngleAxis(dRotationX * rotationSpeed, transform.up) *  //Rotates on X axis
                    Quaternion.AngleAxis(dRotationY * rotationSpeed, transform.right) *                                 //Rotates on Y axis
                    inspectingObject.transform.rotation;                                                                //Multiplying the original rotation
            }
        }
    }
}
