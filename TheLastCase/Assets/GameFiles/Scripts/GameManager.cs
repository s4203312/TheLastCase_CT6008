using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject sparkleEffect;

    private void Start()
    {
        UIHints.Instance.ShowMessages(new List<(string, float)>
        {
            ("THE LAST CASE", 2f),
            ("Use the mouse to move and interact, either holding or clicking.", 4f),
            ("Get close to objects, inspect and interact to solve the puzzle and escape!", 4f)
        });

        InteractableObject[] interactableObjects = FindObjectsOfType<InteractableObject>();

        foreach (InteractableObject interactableObject in interactableObjects)
        {
            GameObject interObj = interactableObject.gameObject;

            if (interObj.GetComponent<InteractableObject>().itemData != null )
            {
                GameObject particleEffect = Instantiate(sparkleEffect, interObj.transform);
                particleEffect.tag = "Untagged";
            }
        }
    }
}
