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
        Magical
    }

    public Item(string name, int id, string description, int attack, int defense, int health, int value, ItemType type)
    {
        itemName = name;
        itemID = id;
        itemDescription = description;
        itemIcon = Resources.Load<Texture2D>("ItemIcons/" + name);
        itemAttack = attack;
        itemDefense = defense;
        itemHealth = health;
        itemValue = value;
        itemType = type;
    }

    public Item()
    {
        //slot constructor
    }
}
