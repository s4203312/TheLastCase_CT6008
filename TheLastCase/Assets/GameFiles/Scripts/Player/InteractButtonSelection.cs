using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractButtonSelection : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Button playerButton;
    [SerializeField] private string ActionName;
    

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
        script.StartCoroutine(ActionName);
    }
}
