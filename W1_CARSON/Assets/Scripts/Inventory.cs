using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int slotsX = 4;
    public int slotsY = 5;
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
        }
        
        _database = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<ItemDatabase>();
        inventory.Add(_database.itemList[0]);
        inventory.Add(_database.itemList[3]);
        inventory.Add(_database.itemList[4]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            _showInventory = !_showInventory;
            Debug.Log("Inventory!");
        }    
    }
    
    private void OnGUI()
    {
        if (_showInventory)
        {
            DrawInventory();
        }
        for (int i = 0; i < inventory.Count; i++)
        {
            GUI.Label(new Rect(10, i * 20, 200, 50), inventory[i].itemName);
        }
    }

    void DrawInventory()
    {
        for (int x = 0; x < slotsX; x++)
        {
            for (int y = 0; y < slotsY; slotsY++)
            {
                GUI.Box(new Rect(x * 20, y * 20, 20, 20), y.ToString());
            }
        }
    }
    
}
