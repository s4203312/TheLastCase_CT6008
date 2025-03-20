using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;

public class UIInventoryLoad : MonoBehaviour
{
    public GameObject InventoryPanelUI;
    public InventoryManager inventoryManager;
    public Sprite nullSprite;

    public GameObject playerCharacter;
    private InteractActions interactActions;
    public GraphicRaycaster UIraycaster;

    private bool localInspectingInventory;

    private void Start()
    {
        interactActions = playerCharacter.GetComponent<InteractActions>();

        inventoryManager = inventoryManager.GetComponent<InventoryManager>();
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            UIraycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                GameObject hitObject = result.gameObject;

                if (hitObject.CompareTag("InventoryImage"))
                {
                    string itemPosition = string.Concat(hitObject.name[..1]);
                    Debug.Log(itemPosition);

                    if (localInspectingInventory)
                    {
                        // inspect item functionaility
                        Debug.Log("Insepct");
                    }
                    else
                    {
                        Debug.Log("Place");
                        interactActions.PlaceItem(itemPosition);            
                    }       
                }
            }
        }
    }

    public void LoadInventory(bool inspectingInventory)
    {
        //Setting panel active

        InventoryPanelUI.SetActive(true);
        localInspectingInventory = inspectingInventory;

        //Clearing the inventory slots           
        for (int i = 0; i < 8; i++)
        {
            GameObject slot = InventoryPanelUI.transform.GetChild(i).gameObject;

            //Clearing the inventory slots
            slot.transform.GetChild(0).GetComponent<Image>().sprite = nullSprite;
            slot.transform.GetChild(1).GetComponent<TMP_Text>().text = null;
            slot.transform.GetChild(2).GetComponent<TMP_Text>().text = null;
        }



        //Get Inventory or Refresh the variable if not opening for first time
        List<InventoryItemData> Inventory = inventoryManager.Inventory;

        
        //Finding the inventory slots           
        for (int i = 0; i < Inventory.Count; i++)
        {
            GameObject slot = InventoryPanelUI.transform.GetChild(i).gameObject;

            //Filling the inventory slots
            slot.transform.GetChild(0).GetComponent<Image>().sprite = Inventory[i].itemPicture;
            slot.transform.GetChild(1).GetComponent<TMP_Text>().text = Inventory[i].itemName;
            slot.transform.GetChild(2).GetComponent<TMP_Text>().text = Inventory[i].itemDescription;
        }
    }

    public void InventoryClose()
    {
        //Setting panel deactive
        InventoryPanelUI.SetActive(false);
    }
}
