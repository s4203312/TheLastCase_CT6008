using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent player;
    public GameObject interactedObject;

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
                    
                    player.SetDestination(hit.point);
                }
            }
        }
    }
}
