using Cinemachine;
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

    public bool isGhostActive;

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

            Camera gameCam = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>().OutputCamera;

            Ray ray = gameCam.ScreenPointToRay(Input.mousePosition);
            //Ray ray = CinemachineCore.Instance.GetActiveBrain(0).OutputCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 10.0f);
                Debug.Log(hit.point);
                if (hit.transform.GetComponent<NavMeshSurface>() || hit.transform.tag == "Interactable") 
                {
                    if (!isGhostActive)
                    {
                        Debug.Log("player move");
                        player.SetDestination(hit.point);
                    }
                    else if(isGhostActive)
                    {
                        Debug.Log("ghost move");
                        ghost.SetDestination(hit.point);        //Doesnt move player as tethered to player
                    }
                    
                }
            }
        }

        if (Input.GetMouseButtonDown(1))          //Switch between ghost and player
        {
            if (isGhostActive)
            {
                playerController.isGhostActive = false;

                //Player components enabling
                player.GetComponent<PlayerMovement>().enabled = true;

                //Ghost components disabling
                ghost.GetComponent<PlayerMovement>().enabled = false;

                ghost.gameObject.SetActive(false);
            }
            else
            {
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
