using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController playerController;

    [SerializeField] private NavMeshAgent player;
    [SerializeField] private NavMeshAgent ghost;
    public GameObject interactedObject;

    private bool isGhostActive;

    private void Awake()
    {
        isGhostActive = false;
    }

    void Start()
    {
        isGhostActive = playerController.isGhostActive;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }      

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.GetComponent<NavMeshSurface>() || hit.transform.tag == "Interactable") 
                {
                    //Vector3 targetDirection = hit.transform.position - player.transform.position;
                    //Vector3.RotateTowards(player.transform.forward, targetDirection, Time.deltaTime, 0.0f);
                    Debug.Log(isGhostActive);
                    if (!isGhostActive)
                    {
                        player.SetDestination(hit.point);
                    }
                    else if(isGhostActive)
                    {
                        ghost.SetDestination(hit.point);        //Doesnt move player as tethered to player
                    }
                    
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            if (isGhostActive)
            {
                Debug.Log("This is now false");
                playerController.isGhostActive = false;
                //Player components enabling
                player.GetComponent<PlayerMovement>().enabled = true;

                //Ghost components disabling
                ghost.GetComponent<PlayerMovement>().enabled = false;

                ghost.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("This is now true");
                playerController.isGhostActive = true;
                ghost.transform.position = player.transform.position;        //Moves ghost too player position to ensure its in same place when switch happens
                
                //Player components disabling
                player.GetComponent<PlayerMovement>().enabled = false;

                //Ghost components disabling
                ghost.GetComponent<PlayerMovement>().enabled = true;

                ghost.gameObject.SetActive(true);
            }
        }
    }
}
