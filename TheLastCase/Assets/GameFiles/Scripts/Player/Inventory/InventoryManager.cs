using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public bool inspectingInventory = false;

    //List for the inventory of the player
    public List<InventoryItemData> Inventory = new List<InventoryItemData>();

    private void Awake()
    {

        if(Instance == null)
        {
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    //Adding item to list
    public void AddItemToInventory(InventoryItemData item)
    {
        if (!Inventory.Contains(item))
        {
            Inventory.Add(item);
            //Debug.Log("Item Added");
        }
    }

    //Removing item from list
    public void RemoveItemFromInventory(InventoryItemData item)
    {
        if (Inventory.Contains(item))
        {
            Inventory.Remove(item);
        }
    }

    //Checking if item is in list
    public static bool IsItemInInventory(InventoryItemData item)
    {
        return Instance != null && Instance.Inventory.Contains(item);
    }

    public void IsInspectingInventory(bool value) { inspectingInventory = value; }
}
