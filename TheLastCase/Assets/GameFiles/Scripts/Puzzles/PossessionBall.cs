using UnityEngine;

public class PossessionBall : MonoBehaviour
{
    public GameObject ballPosStart;

    // Break ball if hits books
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Books") 
        { 
            transform.position = ballPosStart.transform.position; 
            GetComponent<GhostPosession>().selectedItem = null;
            GetComponent<AudioSource>().Play();
        }
    }
}
