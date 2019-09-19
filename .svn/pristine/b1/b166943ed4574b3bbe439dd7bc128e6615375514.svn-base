using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Abstract Classes

[System.Serializable]
public abstract class Item
{
    public string name;
    public int id;
    public string description;
    public Texture2D icon;
    public int itemHealth;
    public int value;
    
    protected Item(string name, int id, string description, int value)
    {
        this.name = name;
        this.id = id;
        this.description = description;
        icon = Resources.Load<Texture2D>("ItemIcons/" + name);
        this.value = value;
    }

    public abstract void Select();
}
public abstract class ItemDefinition : ScriptableObject
{
    public virtual Type type { get { return typeof(Item); }}
    public string name;
    public int id;
    public int value;
    public Sprite icon;
}

public abstract class Equipable : Item
{
    public Equipable(string name, int id, string description, int value) : base(name, id, description, value) { }

    public abstract void Equip();
}
public abstract class EquipableDefinition : ItemDefinition
{
    public override Type type { get { return typeof(Equipable); } }
}


public abstract class Consumable : Item
{
    public Consumable(string name, int id, string description, int value) : base(name, id, description, value) { }

    public abstract void Consume();
}

public abstract class ConsumableDefinition : ItemDefinition
{
    public override Type type { get { return typeof(Consumable); } }
}

#endregion

public class Weapon : Equipable
{
    public int attackValue;

    public Weapon(string name, int id, string description, int value, int attackValue) : base(name, id, description, value)
    {
        this.attackValue = attackValue;
    }

    public override void Select()
    {
        // Check if in inventory
        // Do a level check 
        // Check character type;
        
        Equip();
    }

    public override void Equip()
    {
        // make an equip sound
        // put on to the character
        // anything else
    }
}
[CreateAssetMenu (menuName = "Weapon Definition")]
public class WeaponDefinition : EquipableDefinition
{
    public int attackValue;
    
    public override Type type { get { return typeof(Weapon); } }
}

public  class Armor : Equipable
{
    public int defenseValue;

    public Armor(string name, int id, string description, int value, int defenseValue) : base(name, id, description, value)
    {
        this.defenseValue = defenseValue;
    }

    public override void Select()
    {
        Equip();
    }

    public override void Equip()
    {
        // make an equip sound
        // put on to the character
        // anything else
    }
}
[CreateAssetMenu (menuName = "Weapon Definition")]
public class ArmorDefinition : EquipableDefinition
{
    public int defenseValue;
    
    public override Type type { get { return typeof(Armor); } }
}

/*
public class Potion : Consumable
{
    public int healthIncrease;
    public bool statusEffectOrStat;
    public Stat affectedStat;
    public StatusEffect affectedStatus;
    
    public HealthPotion(string name, int id, string description, int value, int healthIncrease) : base(name, id, description, value)
    {
        this.healthIncrease = healthIncrease;
    }

    public override void Select()
    {
        Consume();
    }

    public override void Consume()
    {
        // player health increases by health increase
    }
}
public class PotionDefinition : ConsumableDefinition
{
}
*/

public class Spell : Consumable
{
    public Spell(string name, int id, string description, int value) : base(name, id, description, value)
    {
    }

    public override void Select()
    {
        Consume();
    }

    public override void Consume()
    {
        // learn spell?
        // attack w/ spell?
    }
}
public class SpellDefinition : ConsumableDefinition
{
    public override Type type { get { return typeof(Spell); } }
}