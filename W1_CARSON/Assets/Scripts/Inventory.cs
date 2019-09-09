using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();

    private ItemDatabase _database;
    
    // Start is called before the first frame update
    void Start()
    {
        _database = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<ItemDatabase>();
        
        inventory.Add(_database.itemList[0]);
        inventory.Add(_database.itemList[3]);
        inventory.Add(_database.itemList[4]);
    }

    private void OnGUI()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            GUI.Label(new Rect(10, i * 20, 200, 50), inventory[i].itemName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
