using Cinemachine;
using System.Collections;
using System;
using Unity.AI.Navigation;
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

    public GameObject playerPointShader;
    public bool ghostPullingBackIn = false;
    public AudioClip footstepsSFX;
    public AudioClip transformSFX;

    private CinemachineBrain gameCam;
    private CinemachineVirtualCamera virtualCam;

    private void Awake()
    {
        playerController.isGhostActive = false;
        virtualCam = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }


    private void Update()
    {
        //Getting audio source information for footsteps
        AudioSource playerAudio = player.transform.parent.GetComponent<AudioSource>();
        if (player.velocity.magnitude < 0.1f)
        {
            if(playerAudio.clip != transformSFX)
            {
                playerAudio.Stop();
                playerAudio.clip = null;
                playerAudio.loop = false;
            }
        }

        if (Input.GetMouseButton(0))
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
                if (hit.transform.GetComponent<NavMeshSurface>() || hit.transform.tag == "Interactable")
                {
                    //Destroy any left over shader before moving again
                    Destroy(GameObject.Find("PlayerPointShader(Clone)"));

                    if (!playerController.isGhostActive)
                    {
                        //Play footstep SFX
                        if (!playerAudio.isPlaying)
                        {
                            playerAudio.clip = footstepsSFX;
                            playerAudio.loop = true;
                            playerAudio.Play();
                        }

                        player.SetDestination(hit.point);

                        Vector3 shaderSpawnPos = new Vector3(hit.point.x, 0, hit.point.z);
                        Instantiate(playerPointShader, shaderSpawnPos, Quaternion.identity);
                    }
                    else if (playerController.isGhostActive)
                    {
                        ghost.SetDestination(hit.point);        //Doesnt move player as tethered to player
                        Vector3 shaderSpawnPos = new Vector3(hit.point.x, 0, hit.point.z);
                        Instantiate(playerPointShader, shaderSpawnPos, Quaternion.identity);
                    }

                }
            }
        }

        if (Input.GetMouseButtonDown(1))          //Switch between ghost and player
        {
            interactButton.gameObject.SetActive(false); //Making button turn off to reduce errors

            //Fixing the loop for footsteps
            playerAudio.clip = transformSFX;
            playerAudio.loop = false;
            playerAudio.Play();

            if (playerController.isGhostActive)
            {
                playerController.isGhostActive = false;

                ghost.gameObject.SetActive(false);
                GetComponent<GhostTetherRenderer>().enabled = false;
                GameObject.Find("Main Camera").transform.GetChild(0).gameObject.SetActive(false);       //Screen shader unactive

                if (virtualCam.Follow != null)
                {
                    virtualCam.Follow = player.transform;
                }
            }
            else
            {
                StopPlayer();

                playerController.isGhostActive = true;
                ghost.transform.position = player.transform.position - (player.transform.forward * 1.2f);        //Moves ghost too player position to ensure its in same place when switch happens

                ghost.gameObject.SetActive(true);
                GetComponent<GhostTetherRenderer>().enabled = true;
                GameObject.Find("Main Camera").transform.GetChild(0).gameObject.SetActive(true);        //Screen shader active
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
            if (distanceAway > 25)
            {
                //Pulling ghost back in
                ghostPullingBackIn = true;
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
            float duration = Vector3.Distance(startPos, targetPos) / pullSpeed; //Time required based on speed

            while (elapsedTime < duration)
            {
                ghost.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        ghostPullingBackIn = false;
        GetComponent<GhostTetherRenderer>().linePoints.Clear();
        playerController.isGhostActive = false;
        ghost.gameObject.SetActive(false);
        ghost.GetComponent<NavMeshAgent>().enabled = true;
        GameObject.Find("Main Camera").transform.GetChild(0).gameObject.SetActive(false);       //Screen shader unactive

        if (virtualCam.Follow != null)
        {
            virtualCam.Follow = player.transform;
        }
    }

    public void StopPlayer()
    {
        player.SetDestination(player.transform.position);
    }
}
