using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A scriptable object that can be used to defind new object information with in game
//Can be very reuseable when multiple object have same name and description

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemPicture;
    public GameObject itemObject; // remove and add second list for inventory gameobjects
    public string puzzleID;
    //Can put other things here like UI pic

    public InventoryItemData(string itemName, string itemDescription, Sprite itemPicture, GameObject itemObject, string puzzleID)
    {
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.itemPicture = itemPicture;
        this.itemObject = itemObject;
        this.puzzleID = puzzleID;
    }
}
