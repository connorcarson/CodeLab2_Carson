using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public int itemID;
    public string itemDescription;
    public Texture2D itemIcon;
    public int itemAttack;
    public int itemDefense;
    public int itemHealth;
    public int itemValue;
    public ItemType itemType;
    
    public enum ItemType
    {
        Weapon,
        Consumable,
        Quest
    }
}
