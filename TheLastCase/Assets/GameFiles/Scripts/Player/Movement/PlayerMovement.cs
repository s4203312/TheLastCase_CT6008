using Cinemachine;
using System.Collections;
using System;
using System.Linq;
using Unity.AI.Navigation;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController playerController;

    [SerializeField] private NavMeshAgent player;
    [SerializeField] private NavMeshAgent ghost;
    public GameObject interactedObject;
    public Button interactButton;

    //Can change this to use a assets:// file link??
    public GameObject playerPointShader;

    private CinemachineBrain gameCam;
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

            gameCam = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();

            gameCam.OutputCamera.transform.position = virtualCam.transform.position;
            gameCam.OutputCamera.transform.rotation = virtualCam.transform.rotation;

            Ray ray = gameCam.OutputCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 10.0f);
                if (hit.transform.GetComponent<NavMeshSurface>() || hit.transform.tag == "Interactable") 
                {
                    //Destroy any left over shader before moving again
                    Destroy(GameObject.Find("PlayerPointShader(Clone)"));

                    if (!playerController.isGhostActive)
                    {
                        player.SetDestination(hit.point);

                        Vector3 shaderSpawnPos = new Vector3(hit.point.x, 1.154952f, hit.point.z);
                        Instantiate(playerPointShader, shaderSpawnPos, Quaternion.identity);
                    }
                    else if(playerController.isGhostActive)
                    {
                        ghost.SetDestination(hit.point);        //Doesnt move player as tethered to player
                        Vector3 shaderSpawnPos = new Vector3(hit.point.x, 1.154952f, hit.point.z);
                        Instantiate(playerPointShader, shaderSpawnPos, Quaternion.identity);
                    }
                    
                }
            }
        }

        if (Input.GetMouseButtonDown(1))          //Switch between ghost and player
        {
            interactButton.gameObject.SetActive(false); //Making button turn off to reduce errors

            if (playerController.isGhostActive)
            {
                playerController.isGhostActive = false;

                ghost.gameObject.SetActive(false);
                GetComponent<GhostTetherRenderer>().enabled = false;

                if (virtualCam.Follow != null)
                {
                    virtualCam.Follow = player.transform;
                }
            }
            else
            {
                playerController.isGhostActive = true;
                ghost.transform.position = player.transform.position;        //Moves ghost too player position to ensure its in same place when switch happens

                ghost.gameObject.SetActive(true);
                GetComponent<GhostTetherRenderer>().enabled = true;
                GetComponent<GhostTetherRenderer>().StartGhostPath();

                if (virtualCam.Follow != null)
                {
                    virtualCam.Follow = ghost.transform;
                }
            }
        }

        if (playerController.isGhostActive)         //Checking to pull ghost back in
        {
            float distanceAway = Vector3.Distance(player.transform.position, ghost.transform.position);
            if (distanceAway > 10)
            {
                //Pulling ghost back in
                Vector3[] tetherPoints = GetComponent<GhostTetherRenderer>().linePoints.ToArray();
                Array.Reverse(tetherPoints);
                ghost.GetComponent<NavMeshAgent>().enabled = false;
                StartCoroutine(PullGhostBackIn(tetherPoints));
            }
        }
    }

    public IEnumerator PullGhostBackIn(Vector3[] tetherPoints)
    {
        foreach (var tetherPoint in tetherPoints)
        {
            Vector3 targetPos = tetherPoint;
            Vector3 startPos = ghost.transform.position;
            float pullSpeed = 10f;
            float elapsedTime = 0f;
            float duration = Vector3.Distance(startPos, targetPos) / pullSpeed; // Time required based on speed

            while (elapsedTime < duration)
            {
                ghost.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        playerController.isGhostActive = false;
        ghost.gameObject.SetActive(false);
        ghost.GetComponent<NavMeshAgent>().enabled = true;

        if (virtualCam.Follow != null)
        {
            virtualCam.Follow = player.transform;
        }
    }
}
