using UnityEngine;

public class RotateObject : MonoBehaviour       //Ceiling fan spinning
{
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.down, 1f);
    }
}