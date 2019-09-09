using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int slotsX = 4;
    public int slotsY = 5;
    public GUISkin slotSkin;
    private bool _showInventory;
    private bool _showTooltip;
    private string _tooltip;
    
    public List<Item> inventory = new List<Item>();
    public List<Item> slots = new List<Item>();

    private ItemDatabase _database;
    
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < (slotsX * slotsY); i++)
        {
            slots.Add(new Item());
            inventory.Add(new Item());
        }
        
        _database = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<ItemDatabase>();
        
        AddItem(1);
        AddItem(2);
        AddItem(4);
        
        print(InventoryContains(3));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            _showInventory = !_showInventory;
            //Debug.Log("You've pressed the inventory button!");
        }

        if (Input.GetKey(KeyCode.R))
        { 
            Debug.Log("delete!");
            RemoveItem(2);
        }
    }
    
    private void OnGUI()
    {
        _tooltip = "";
        GUI.skin = slotSkin;
         
        if (_showInventory)
        {
            DrawInventory();
        }

        if (_showTooltip)
        {
            GUI.Box(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 200, 200), _tooltip, slotSkin.GetStyle("Tooltip"));
        }
    }

    void DrawInventory()
    {
        int i = 0;
        
        for (int y = 0; y < slotsY; y++)
        {
            for (int x = 0; x < slotsX; x++)
            {
                Rect slotRect = new Rect(x * 105, y * 105, 100, 100 );
                //Rect itemRect = new Rect(x * 105 + 15, y * 105 + 15, 70, 70); //smaller icons with offset
                GUI.Box(slotRect, y.ToString(), slotSkin.GetStyle("Slot"));

                slots[i] = inventory[i];

                if (slots[i].itemName != null)
                {
                    GUI.DrawTexture(slotRect, slots[i].itemIcon);
                    if(slotRect.Contains(Event.current.mousePosition))
                    {
                        _showTooltip = true;
                        _tooltip = CreateTooltip(slots[i]);
                    }
                }

                if (_tooltip == "")
                {
                    _showTooltip = false;
                }
                
                i++;
            }
        }
    }

    string CreateTooltip(Item item)
    {
        _tooltip = item.itemName;
        return _tooltip;
    }

    void AddItem(int id)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == null)
            {
                for (int j = 0; j < _database.itemList.Count; j++)
                {
                    if (_database.itemList[j].itemID == id)
                    {
                        inventory[i] = _database.itemList[j];   
                    }
                }
                break;
            }
        }
    }

    void RemoveItem(int id)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemID == id)
            {
                inventory[i] = new Item();
                break;
            }
        }
    }

    bool InventoryContains(int id)
    {
        bool result = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            result = inventory[i].itemID == id;
            if (result)
            {
                break;
            }
        }
        return result;
    }

}
