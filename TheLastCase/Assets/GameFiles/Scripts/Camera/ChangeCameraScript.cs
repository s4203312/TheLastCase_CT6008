using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraScript : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera otherCam;

    private void Start() {
        mainCam.enabled = true;
        otherCam.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player") {
            mainCam.enabled = false;
            otherCam.enabled = true;
        }
    }    

    public void BackButton() {
        mainCam.enabled = true;
        otherCam.enabled = false;
    }
}
