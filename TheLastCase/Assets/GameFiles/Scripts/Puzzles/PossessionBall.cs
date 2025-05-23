using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessionBall : MonoBehaviour
{
    private Vector3 ballPosStart;

    private void Start()
    {
        ballPosStart = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Books") 
        { 
            transform.position = ballPosStart; 
            gameObject.GetComponent<GhostPosession>().selectedItem = null;
        }
    }
}
