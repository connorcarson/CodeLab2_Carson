using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    public List<Item> slots = new List<Item>();
    
    public int slotsX = 4;
    public int slotsY = 5;
    public GUISkin slotSkin;
    
    private ItemDatabase _database;
    private int _prevIndex;
    private float _iconWidth = 100;
    private float _iconHeight = 100;
    private GUIStyle _style;
    private Vector2 _size;
    private bool _showInventory;
    private bool _showTooltip;
    private bool _draggingItem;
    private string _tooltip;
    private Item _draggedItem;

    // Start is called before the first frame update
    void Start()
    {
        //for the grid defined by the values slotsX and slotsY
        for (int i = 0; i < (slotsX * slotsY); i++)
        {
            //add a slot
            slots.Add(new Item());
            //add an empty item constructor)
            inventory.Add(new Item());
        }
        
        //get our reference to our Item database
        _database = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<ItemDatabase>();
        
        //Add the following items to our inventory from the start
        AddItem(1);
        AddItem(2);
        AddItem(4);
        AddItem(3);
    }

    // Update is called once per frame
    void Update()
    {
        //if we've pressed the inventory button "I"
        if (Input.GetButtonDown("Inventory"))
        {
            //set our boolean to the opposite of its current state
            _showInventory = !_showInventory;
        }

        //if we've pressed the "R" button
        /*if (Input.GetKey(KeyCode.R))
        { 
            //remove the define Item
            Debug.Log("delete!");
            RemoveItem(2);
        }*/
    }
    
    private void OnGUI()
    {
        //reset the tooltip content to be empty
        _tooltip = "";
        //get our reference to our custom GUISkin
        GUI.skin = slotSkin;
        
        //if we're currently showing the inventory
        if (_showInventory)
        {
            //draw the inventory
            DrawInventory();
            
            //if we're currently showing a tooltip
            if (_showTooltip)
            {
                //set the content to be equal to our tooltip string
                GUIContent content = new GUIContent(_tooltip);
                
                //set our tooltip parameters according to our aesthetic preference
                _style = GUI.skin.box;
                _style.fontSize = 18;
                _style.alignment = TextAnchor.UpperLeft;
                //calculate the size of the tooltip box based on the content (the size of the string)
                _size = _style.CalcSize(content);
                
                //draw our tooltip at the position of our mouse according to the size and style determined above
                GUI.Box(new Rect(Event.current.mousePosition.x + 20f, Event.current.mousePosition.y, _size.x, _size.y), _tooltip, _style);
            }
            
            //if we're currently dragging an item
            if (_draggingItem)
            {
                //draw the icon of that item at the position of our mouse according to the size of the original icon
                GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, _iconWidth, _iconHeight), _draggedItem.itemIcon);
            }
        }
    }

    void DrawInventory()
    {
        Event e = Event.current;
        int i = 0;
        
        //for every row in our inventory
        for (int y = 0; y < slotsY; y++)
        {
            //and for every slot in every row
            for (int x = 0; x < slotsX; x++)
            {
                //define the placement and size of our slots
                Rect slotRect = new Rect(x * 105, y * 105, 100, 100 );
                //draw each slot according to our defined size, placement and custom GUIskin
                GUI.Box(slotRect, "", slotSkin.GetStyle("Slot"));
                
                //set empty slots to correspond to empty item constructors
                slots[i] = inventory[i];
                
                //if a slot is NOT empty
                if (slots[i].itemName != null)
                {
                    //draw the item in that slot
                    GUI.DrawTexture(slotRect, slots[i].itemIcon);
                    //if the mouse cursor is within that slot
                    if(slotRect.Contains(e.mousePosition))
                    {
                        //and if the left mouse button is pressed AND the mouse is dragged AND we're not already dragging an item
                        if (e.button == 0 && e.type == EventType.MouseDrag && !_draggingItem)
                        {
                            _draggingItem = true;
                            //store the index of the item we're dragging
                            _prevIndex = i;
                            //set the dragged item to the item we clicked on
                            _draggedItem = slots[i];
                            //put an empty item constructor in the slot we've just emptied
                            inventory[i] = new Item();
                        }
                        //or if the left mouse button is released or clicked AND we're already dragging an item
                        if(e.type == EventType.MouseUp && _draggingItem)
                        {
                            //the item our cursor is hovering over goes into the slot where we previously grabbed the item we're dragging
                            inventory[_prevIndex] = inventory[i];
                            //the item we're currently dragging goes into the slot our cursor is hovering over
                            inventory[i] = _draggedItem;
                            _draggingItem = false;
                            _draggedItem = null;
                        }
                        //or if we right click
                        if (e.isMouse && e.type == EventType.MouseDown && e.button == 1)
                        {
                            //and the item in the slot is a consumable item
                            if (slots[i].itemType == Item.ItemType.Consumable)
                            {
                                //use that item
                                UseItem(slots[i], i, true);
                            }
                        }
                        //or if we're not currently dragging an item
                        if (!_draggingItem)
                        {
                            //show the tooltip based on the information defined by our CreateTooltip Function
                            _showTooltip = true;
                            _tooltip = CreateTooltip(slots[i]);
                        }
                    }
                }
                else //if a slot IS empty
                {  
                    //and the mouse cursor is within that slot
                    if (slotRect.Contains(e.mousePosition))
                    {
                        //and the left mouse button is released or clicked AND we're already dragging an item
                        if (e.type == EventType.MouseUp && _draggingItem)
                        {
                            //the item we're currently dragging goes into the slot our cursor is hovering over
                            inventory[i] = _draggedItem;
                            _draggingItem = false;
                            _draggedItem = null;
                        }
                    }
                }
                //if there's no information in our tooltip string
                if (_tooltip == "")
                {
                    //don't show the tooltip
                    _showTooltip = false;
                }
                
                i++;
            }
        }
    }

    string CreateTooltip(Item item)
    {
        switch (item.itemType)
        {   
            //if the item type is a weapon
            case Item.ItemType.Weapon:
                //use the following stats/properties in the tooltip
                _tooltip = "<color=#E9E9E9><b>" + item.itemName + "</b></color>\n\n" + 
                           "<color=#E9E9E9>" + item.itemDescription + 
                           "\n\nAttack: " + "</color>" + "<color=#DA5151>" + item.itemAttack + "</color>" + 
                           "<color=#E9E9E9>" + "\nDefense: " + "</color>" + "<color=#67AFC6>" + item.itemDefense + "</color>" + 
                           "<color=#E9E9E9>" + "\nValue: " + "</color>" + "<color=#E0CA6D>" + item.itemValue + "</color>";
                break;
            //if the item type is magical
            case Item.ItemType.Magical:
                //etc.
                _tooltip = "<color=#E9E9E9><b>" + item.itemName + "</b></color>\n\n" + 
                           "<color=#E9E9E9>" + item.itemDescription + 
                           "\n\nAttack: " + "</color>" + "<color=#DA5151>" + item.itemAttack + "</color>" + 
                           "<color=#E9E9E9>" + "\nDefense: " + "</color>" + "<color=#67AFC6>" + item.itemDefense + "</color>" +
                           "<color=#E9E9E9>" + "\nHealth: " + "</color>" + "<color=#7DC062>" + item.itemHealth + "</color>" +
                           "<color=#E9E9E9>" + "\nValue: " + "</color>" + "<color=#E0CA6D>" + item.itemValue + "</color>";
                break;
            //if the item type is a consumable
            case Item.ItemType.Consumable:
                //etc.
                _tooltip = "<color=#E9E9E9><b>" + item.itemName + "</b></color>\n\n" + 
                           "<color=#E9E9E9>" + item.itemDescription + "</color>" +
                           "<color=#E9E9E9>" + "\nHealth: " + "</color>" + "<color=#7DC062>" + item.itemHealth + "</color>" +
                           "<color=#E9E9E9>" + "\nValue: " + "</color>" + "<color=#E0CA6D>" + item.itemValue + "</color>";
                break;
            default:
                break;
        }
        return _tooltip;
    }

    void AddItem(int id)
    {    
        //loop through every element in our inventory
        for (int i = 0; i < inventory.Count; i++)
        {
            //when you find an empty element
            if (inventory[i].itemName == null)
            {
                //loop through every item in our database
                for (int j = 0; j < _database.itemList.Count; j++)
                {
                    //when you find the item in our database whose id matches the id defined by our parameter
                    if (_database.itemList[j].itemID == id)
                    {
                        //add that item into the empty slot
                        inventory[i] = _database.itemList[j];   
                    }
                }
                break;
            }
        }
    }

    void RemoveItem(int id)
    {
        //loop through every element in our inventory
        for (int i = 0; i < inventory.Count; i++)
        {
            //when you find the item in our inventory whose id matches the id defined by our parameter
            if (inventory[i].itemID == id)
            {
                //replace that item with an empty item constructor
                inventory[i] = new Item();
                break;
            }
        }
    }

    void UseItem(Item item, int slot, bool deleteItem)
    {
        //this is an example of how this function could be used to different affect if we had a Stats class (or something similar) that was affected
        //by different kinds of items
        switch (item.itemID)
        {
            case 4:
                /*PlayerStats.IncreaseStat(3, 15, 50f)*/
                Debug.Log("Used consumable: " + item.itemName);
                break;
            default:
                break;
        }
        
        //if the item is "deletable"
        if (deleteItem)
        {
            //replace the item defined by our parameters with an empty item constructor
            inventory[slot] = new Item();
        }
    }

    bool InventoryContains(int id)
    {
        bool result = false;
        //loop through every element in our inventory
        for (int i = 0; i < inventory.Count; i++)
        {
            //if the id of an item matches the id defined by our parameter than result is true
            result = inventory[i].itemID == id;
            if (result)
            {
                break;
            }
        }
        return result;
    }

}
