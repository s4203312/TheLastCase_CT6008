using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent player;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.GetComponent<NavMeshSurface>() || hit.transform.tag == "Interactable") 
                {
                    Debug.Log(hit.point);
                    player.SetDestination(hit.point);
                    Debug.Log(player.destination);
                }
            }
        }
    }
}
