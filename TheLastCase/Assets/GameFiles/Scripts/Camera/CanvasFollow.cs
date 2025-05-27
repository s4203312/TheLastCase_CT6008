using UnityEngine;

public class CanvasFollow : MonoBehaviour
{
    //Inspector Variables
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject ghost;
    [SerializeField] private bool Camera;
    public PlayerController playerController;

    //Private Variables
    private Vector3 cameraPerspective;
    private Vector3 canvasPerspective;
    private bool isGhostActive;


    private void Start()
    {
        cameraPerspective = new Vector3(3f, 12.5f, 0.2f);
        canvasPerspective = new Vector3(0.0f, 3f, 0.0f);
    }

    private void Update()
    {
        isGhostActive = playerController.isGhostActive;

        //Checking if cavnas or camera is following player or ghost
        if (isGhostActive)
        {
            if (Camera)
            {
                transform.position = ghost.transform.position + cameraPerspective;
            }
            else
            {
                transform.position = ghost.transform.position + canvasPerspective;
            }
        }
        else
        {
            if (Camera)
            {
                transform.position = player.transform.position + cameraPerspective;
            }
            else
            {
                transform.position = player.transform.position + canvasPerspective;
            }
        }
    }
}
