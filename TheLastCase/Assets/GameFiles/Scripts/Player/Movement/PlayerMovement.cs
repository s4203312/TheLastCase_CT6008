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

    private Camera gameCam;
    private CinemachineVirtualCamera virtualCam;

    private void Awake()
    {
        playerController.isGhostActive = false;
        virtualCam = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }


    private void Update()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            gameCam = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>().OutputCamera;
            Ray ray = gameCam.ScreenPointToRay(Input.mousePosition);
            //Ray ray = CinemachineCore.Instance.GetActiveBrain(0).OutputCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 10.0f);
                if (hit.transform.GetComponent<NavMeshSurface>() || hit.transform.tag == "Interactable") 
                {
                    if (!playerController.isGhostActive)
                    {
                        player.SetDestination(hit.point);
                    }
                    else if(playerController.isGhostActive)
                    {
                        ghost.SetDestination(hit.point);        //Doesnt move player as tethered to player
                    }
                    
                }
            }
        }

        if (Input.GetMouseButtonDown(1))          //Switch between ghost and player
        {
            if (playerController.isGhostActive)
            {
                playerController.isGhostActive = false;

                ghost.gameObject.SetActive(false);

                if(virtualCam.Follow != null)
                {
                    virtualCam.Follow = player.transform;
                }
            }
            else
            {
                playerController.isGhostActive = true;
                ghost.transform.position = player.transform.position;        //Moves ghost too player position to ensure its in same place when switch happens

                ghost.gameObject.SetActive(true);

                if (virtualCam.Follow != null)
                {
                    virtualCam.Follow = ghost.transform;
                }
            }
        }
    }
}
