using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.down, 1f);
    }
}