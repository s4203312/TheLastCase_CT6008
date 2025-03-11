using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public Button playerButton;
    [SerializeField] private string ActionName;
    
    //Data stored about item
    public InventoryItemData itemData;


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
        }
    }

    private void FindAction()
    {
        InteractActions script = player.GetComponent<InteractActions>();
        playerButton.onClick.RemoveListener(FindAction);
        script.StartCoroutine(ActionName);
    }
}
