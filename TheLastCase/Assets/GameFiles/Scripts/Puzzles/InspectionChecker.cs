using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionChecker : MonoBehaviour
{
    public InspectObject InspectObjectManager;
    public GameObject InspectObject;

    void Update()
    {
        InspectObjectManager.InspectionFunction(InspectObject, false);
    }
}
