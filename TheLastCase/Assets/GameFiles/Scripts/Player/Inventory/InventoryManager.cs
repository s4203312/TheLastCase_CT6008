using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

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

    //Adding item to dictionary
    public void AddItemToInventory(InventoryItemData item)
    {
        if (!Inventory.Contains(item))
        {
            Inventory.Add(item);
            Debug.Log("Item Added");
        }
    }

    //Removing item from dictionary
    public void RemoveItemFromInventory(InventoryItemData item)
    {
        if (Inventory.Contains(item))
        {
            Inventory.Remove(item);
        }
    }

    //Checking if item is in Dictionary
    public static bool IsItemInInventory(InventoryItemData item)
    {
        return Instance != null && Instance.Inventory.Contains(item);
    }

    //Finds data on specific item
    public static InventoryItemData GetItemData(InventoryItemData item)
    {
        if(Instance != null && Instance.Inventory.Contains(item))
        {
            return Instance.Inventory[1];   //Wrong
        }
        else 
        { 
            return null; 
        }
    }
}
