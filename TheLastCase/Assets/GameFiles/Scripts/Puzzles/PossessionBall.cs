using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessionBall : MonoBehaviour
{
    public GameObject ballPosStart;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Books") 
        { 
            transform.position = ballPosStart.transform.position; 
            gameObject.GetComponent<GhostPosession>().selectedItem = null;
        }
    }
}
