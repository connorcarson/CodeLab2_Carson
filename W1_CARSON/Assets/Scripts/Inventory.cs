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
        _database = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
        
        inventory.Add(_database.itemList[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
