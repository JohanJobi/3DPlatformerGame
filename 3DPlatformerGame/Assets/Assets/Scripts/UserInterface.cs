using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEditor;
using System;

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;
    public Vector2 mouseinputPosition;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();



    void Start()
    {
        //create slots for items and sets up pointer enter and exit events for when the mouse goes in and out
        //of a slot
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }
    public abstract void CreateSlots();//abstract void used here so that it can be
                                       //overrided in Static interface. Serves as a blueprint
    private void OnSlotUpdate(InventorySlot _slot) //updates visuals when slot gets updated
    {
        if (_slot.item.Id >= 0)
        {
            //updates to the sprite only this makes it less memory heavy
            //graphical parts of item are in dataabase and the system part is in the Item class
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.ItemObject.uiDisplay;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            //pure white 100% alpha for an empty slot (we can see the item) 
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == 1 ? "" :
                _slot.amount.ToString("n0");
            //ternary operator used here if slots value amount == 1 then dont display any text if not 1
            //display the amount of items in that slot
        }
        else
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            //alpha 0 so we cannot see the item
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";

        }
    }


    

    //adds an event trigger to a gameobject, allowing the registration for different events
    // protected means it is private but only available to the class its in and any class
    // that inherits from it
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>(); //get even trigger from game object    
        var eventTrigger = new EventTrigger.Entry(); //create new event trigger for the event passed through
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action); // this happens when the specified even happens this
                                                   // means you dont have to loop through every inventory slot
                                                   // to detect an event
        trigger.triggers.Add(eventTrigger);
    }
    //the following are the methods for the mouse on each slot and Interface 
    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }
    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }
    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }
    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj); // at the start a the object is duplicated
    }

    public GameObject CreateTempItem(GameObject obj) // creates temp item for the visual effect that follows
                                                     // the mouse
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }
 
    public void OnDragEnd(GameObject obj)// handles removing of item and swapping (if there is any need)
                                         // between the two slots
    {

        Destroy(MouseData.tempItemBeingDragged);
        if(MouseData.interfaceMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface
                [MouseData.slotHoveredOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }
    public void OnDrag(GameObject obj) //updates the position during dragging to follow the mouse
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

}
public static class MouseData //class containing the mouse data
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}

public static class ExtensionMethods //extension method used so you can add methods
                                     //on to the dictionary slotsOnInterface
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface) //itterates through
                                                                                     //dictionary where
                                                                                     //gameobject is a key
                                                                                     //it returns
                                                                                     //an inventory slot 
        {
            if (_slot.Value.item.Id >= 0)// if an item is the slot
            {
                //updates to the sprite only this makes it less memory heavy
                //graphical parts of item are in dataabase and the system part is in the Item class
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                //pure white 100% alpha for an empty slot (we can see the item) 
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" :
                    _slot.Value.amount.ToString("n0");
                //ternary operator used here if slots value amount == 1 then dont display any text if not 1
                //display the amount of items in that slot
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                //alpha 0 so we cannot see the item
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";

            }
        }
    }
}
 


