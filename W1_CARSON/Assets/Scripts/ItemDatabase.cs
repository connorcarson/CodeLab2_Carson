using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<ItemDefinition> itemList = new List<ItemDefinition>();

    private void Start()
    {
        var itemDescriptions = Resources.LoadAll("Item Definitions/");

        foreach (var itemDescription in itemDescriptions)
        {
            itemList.Add(itemDescription as ItemDefinition);
        }
        
        /*
        itemList.Add(new Item("Fire Sword", 0, "A sword wreathed in white, hot flame.", 8, 6, 0, 195, Item.ItemType.Weapon));
        itemList.Add(new Item("Ice Sword", 1, "A frozen sword sheathed in blue ice.", 11, 2, 0, 255, Item.ItemType.Weapon));
        itemList.Add(new Item("Ruby Amulet", 2, "A glowing red ruby set in a solid gold pendant.", 0, 30, 5, 1255, Item.ItemType.Magical));
        itemList.Add(new Item("Book of Spells", 3, "An old and fading tome written in a nonsensical language.", 0, 0, 0, 5595, Item.ItemType.Magical));
        itemList.Add(new Item("Health Potion", 4, "A bottle of slimy, red goo.", 0, 0, 20, 50, Item.ItemType.Consumable));
        itemList.Add(new Item("Iron Shield", 5, "A sturdy, albeit battle-worn shield wrought in dented iron.", 0, 7, 0, 105, Item.ItemType.Weapon));
   */
    }
}

