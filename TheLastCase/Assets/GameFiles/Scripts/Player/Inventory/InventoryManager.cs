using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    //List for the inventory of the player
    public List<InventoryItemData> Inventory = new List<InventoryItemData>();
    public List<GameObject> InventoryGameObjects = new List<GameObject>();

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
    public void AddItemToInventory(InventoryItemData item, GameObject itemObject)
    {
        if (!Inventory.Contains(item))
        {
            Inventory.Add(item);
            InventoryGameObjects.Add(itemObject);
        }
    }

    //Removing item from list
    public void RemoveItemFromInventory(InventoryItemData item, GameObject itemObject)
    {
        if (Inventory.Contains(item))
        {
            Inventory.Remove(item);
            InventoryGameObjects.Remove(itemObject);
        }
    }

    //Checking if item is in list
    public static bool IsItemInInventory(InventoryItemData item)
    {
        return Instance != null && Instance.Inventory.Contains(item);
    }
}
