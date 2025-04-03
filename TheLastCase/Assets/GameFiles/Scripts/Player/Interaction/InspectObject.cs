using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectObject : MonoBehaviour
{
    public UIInventoryLoad inventoryLoadScipt;
    private GameObject inspectingObject;

    private void Update()
    {
        if (inventoryLoadScipt.isCurrentlyInspecting && Input.GetMouseButton(0))
        {
            inspectingObject = inventoryLoadScipt.currentlyInspectingObject;


            //Perfrom rotation of object with mouse
        }
    }
}
