using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryLoad : MonoBehaviour
{
    public GameObject InventoryPanelUI;
    public InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = inventoryManager.GetComponent<InventoryManager>();
    }

    public void LoadInventory()
    {
        //Get Inventory or Refresh the variable if not opening for first time
        List<InventoryItemData> Inventory = inventoryManager.Inventory;


        
        //Finding the inventory slots           //Ensure maximum 10 slots when adding items!!!!
        for (int i = 0; i < Inventory.Count; i++)
        {
            GameObject slot = InventoryPanelUI.transform.GetChild(i).gameObject;

            //Filling the inventory slots
            slot.transform.GetChild(0).GetComponent<Image>().sprite = Inventory[i].itemPicture;
            slot.transform.GetChild(1).GetComponent<TMP_Text>().text = Inventory[i].itemName;
            slot.transform.GetChild(2).GetComponent<TMP_Text>().text = Inventory[i].itemDescription;
        }
    }
}
