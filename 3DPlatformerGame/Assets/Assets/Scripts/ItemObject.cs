using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Helmet,
    Weapon,
    Shield,
    Boots,
    ChestPlate,
    Mobility,
    Default
}
public abstract class ItemObject : ScriptableObject
{

    public Sprite uiDisplay;
    public bool stackable;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
    public Item data = new Item();

}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id = -1;
    public Item()
    {
        Name = "";
        Id = -1;
    }
    public Item(ItemObject item)
    {
        //every item will have a name and ID
        Name = item.name;
        Id = item.data.Id;

    }
}