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
        
        inventory[0] = _database.itemList[1];
        inventory[1] = _database.itemList[2];
        inventory[2] = _database.itemList[3];

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            _showInventory = !_showInventory;
            //Debug.Log("You've pressed the inventory button!");
        }    
    }
    
    private void OnGUI()
    {
        GUI.skin = slotSkin;
        
        if (_showInventory)
        {
            DrawInventory();
        }
        
        /*for (int i = 0; i < inventory.Count; i++)
        {
            GUI.Label(new Rect(10, i * 20, 200, 50), inventory[i].itemName);
        }*/
    }

    void DrawInventory()
    {
        int i = 0;
        
        for (int y = 0; y < slotsY; y++)
        {
            for (int x = 0; x < slotsX; x++)
            {
                Rect slotRect = new Rect(x * 105, y * 105, 100, 100 );
                //smaller icons with offset
                //Rect itemRect = new Rect(x * 105 + 15, y * 105 + 15, 70, 70);
                GUI.Box(slotRect, y.ToString(), slotSkin.GetStyle("Slot"));

                slots[i] = inventory[i];

                if (slots[i].itemName != null)
                {
                    GUI.DrawTexture(slotRect, slots[i].itemIcon);
                }
                
                i++;
            }
        }
    }
    
}
