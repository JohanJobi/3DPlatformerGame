using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using static UnityEditor.Progress;
using System.Runtime.CompilerServices;
using System.Linq;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]

public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;


    public InventorySlot[] GetSlots { get { return Container.Slots; } }
    //This is a property that returns the array of inventory slots from the container called GetSlots
    public bool AddItem(Item _item, int _amount)
    {//adds item to inventory by amount specified
        if(EmptySlotCount <= 0)
        {
            return false;
        }
        InventorySlot slot = FindItemOnInventory(_item);
        if (!database.GetItem[_item.Id].stackable || slot == null)
        {
            SetEmptySlot(_item, _amount); 
            return true;
        }
        slot.AddAmount(_amount);
        return true;
    }
    public int EmptySlotCount
    { // counts how many empty slots there are by iteration
        get
        {
            int counter = 0;
            for(int i = 0; i< GetSlots.Length; i++)
            {
                if (GetSlots[i].item.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
    public InventorySlot FindItemOnInventory(Item _item)
    {
        //finds item in inventory by iteration
        for(int i = 0;i< GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == _item.Id)
            {
                return GetSlots[i];
            }
        }
        return null;
    }
    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        //sets all slots to have id -1 by iteration
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id <= -1)
            {
                GetSlots[i].UpdateSlot(_item, _amount);
                return GetSlots[i];
            }
        }
        return null;
    }
        
    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {//swapping two items 
        if(item2.CanPlaceInSlot(item1.ItemObject)&& item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }

    }

    public int AmountOfItem(Item _item)
    {//finds amount of item using the information from the inventory slot class that each item inherits
        if (Container != null)
        {
            InventorySlot slot = FindItemOnInventory(_item);
            if (slot != null)
            {
                return slot.amount;
            }
        }
        return 0;



    }

    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter(); //binary formatter so data is unreadable for humans
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        //file stream for writing into text files, persistant path saves it to a path which is consistent between devices for the future of the game 
        formatter.Serialize(stream, Container);
        //serializes container and this gets written into the text file
        stream.Close();

    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath))) //check if file exists 
        {
            IFormatter formatter = new BinaryFormatter(); //new instance of binary formatter for deserialization
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            //deserialization so that it is readable by unity
            int minItemCount = Mathf.Min(GetSlots.Length, GetSlots.Length);
            //Finds the minimum number of slots to iterate through to avoid index out-of-range errors
            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(GetSlots[i].item, GetSlots[i].amount);
            }
            //Iterates through the slots in the loaded
            //inventory and updates the corresponding slots in the current container.
            stream.Close();

        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }

}

public delegate void SlotUpdated(InventorySlot _slot);
[System.Serializable]
public class InventorySlot //represents every slot in the inventory
{
    public ItemType[] AllowedItems = new ItemType[0];//allowed items which are set in the editor
    [System.NonSerialized]
    public UserInterface parent;//parent used so that it doesnt save and makes it so that it is empty
    //reference to the interface the slot belongs to (either equipment or the inventory in this case)
    [System.NonSerialized]
    public GameObject slotDisplay;
    // visuals
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;
    //event that gets triggered after update
    [System.NonSerialized]//non serialized so cannot be seen by unity 
    public SlotUpdated OnBeforeUpdate;
    //event that gets triggered before update
    public Item item;
    public int amount;
    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
        //default slot (with item id -1 and amount 0)
    }
    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
        //updates slot with the item and amount
    }
    public void UpdateSlot(Item _item, int _amount)
    {
        //invoke schedules methods for later times
        if (OnBeforeUpdate != null) // != null used so that it doesnt keep running the function
                                    // if it has already done it
        {
            OnBeforeUpdate.Invoke(this);
        }
        //the "this" keyword uses the current instant of the Inventory slot
        //which is usefull as we have many inventory slots
        item = _item; 
        amount = _amount;
        //updates item and amount
        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this);
        }
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
        //reusing the update slot function with different parameters
        //so you dont need to repeat code with the same logic
    }
    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
        //reusing the update slot function with different parameters
        //so you dont need to repeat code with the same logic
    }
    public bool CanPlaceInSlot(ItemObject _itemObject)
    {//returns bool of if you can place in slot or not
        if (AllowedItems.Length <= 0 || _itemObject == null || _itemObject.data.Id < 0)
        {//if ther is nothing in the slot then you can place in it
            return true;
        }
        for (int i=0; i <AllowedItems.Length; i++)
        {//checks if item type matches with the allowed items list by iteration
            if (_itemObject.type == AllowedItems[i])
            {
                return true;
            }
        }
        return false;
    }
    
    public ItemObject ItemObject
    {
        get
        {
            if(item.Id >= 0)
            {
                return parent.inventory.database.GetItem[item.Id];
            }
            return null;
        }
        //Returns the current item in the slot.
        // this is because the parent contains the inventory's database so retrieves the item from there
    }
}

[System.Serializable]
public class Inventory
{
    public void Clear()
    {//iterates and removes items from all slots
        for(int i=0; i<Slots.Length; i++)
        {
            Slots[i].RemoveItem();
        }
    }

    public InventorySlot[] Slots = new InventorySlot[16]; //used a list with specified size of 16 for the inventory
}
