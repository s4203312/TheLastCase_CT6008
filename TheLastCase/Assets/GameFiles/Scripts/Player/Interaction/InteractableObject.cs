using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    private GameObject player;
    private GameObject playerController;
    private Button playerButton;
    [SerializeField] private string PlayerActionName;
    [SerializeField] private string GhostActionName;
    
    //Data stored about item
    public InventoryItemData itemData;

    public void Start()
    {
        player = GameObject.Find("Player");
        playerController = GameObject.Find("PlayerController");
        playerButton = GameObject.Find("PlayerCanvas").transform.GetChild(0).GetComponent<Button>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player.GetComponent<PlayerMovement>().interactedObject = gameObject;
            playerButton.gameObject.SetActive(true);
            playerButton.onClick.AddListener(FindAction);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player.GetComponent<PlayerMovement>().interactedObject = null;
            playerButton.gameObject.SetActive(false);
            playerButton.onClick.RemoveListener(FindAction);
        }
    }

    private void FindAction()
    {
        InteractActions script = player.GetComponent<InteractActions>();
        playerButton.onClick.RemoveListener(FindAction);

        if (playerController.GetComponent<PlayerController>().isGhostActive)
        {
            if(GhostActionName != "")
            {
                script.StartCoroutine(GhostActionName);
            }
        }
        else
        {
            if (PlayerActionName != "")
            {
                script.StartCoroutine(PlayerActionName);
            }
        }
    }
}
