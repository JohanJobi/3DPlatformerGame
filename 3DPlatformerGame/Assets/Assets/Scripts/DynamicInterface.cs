using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface //this is attatched to the inventory screen so needs the
                                              //same logic from user interface and needs to inherit some
                                              //methods
{
    public int X_SPACE_BETWEEN_ITEM; // inventory graphical properties  
    public int Y_SPACE_BETWEEN_ITEM;
    public int X_START;
    public int Y_START;
    public int NUMBER_OF_COLUMN;
    public GameObject inventoryPrefab;
    public override void CreateSlots() //override makes a new implementation of the method inherited from the base class
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>(); // contains information on which game
                                                                        // object is in which inventory slot
        for (int i = 0; i < inventory.Container.Slots.Length; i++)
        {
            //iterating through slots in inventory 
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });//called when entering
                                                                                     //slot
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });//called when exiting slot
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });//called when starting
                                                                                      //to item from slot
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });//called at the end of the
                                                                                  //drag
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); }); //called when dragging object

            inventory.GetSlots[i].slotDisplay = obj; //updates display for inventory slot
            slotsOnInterface.Add(obj, inventory.GetSlots[i]); // adds the slot and its inventory slot
                                                              // to the dictionary
                                                              
        }
    }
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN), Y_START + (-Y_SPACE_BETWEEN_ITEM * (i / NUMBER_OF_COLUMN)), 0f);
        //calculates poisiton of slots
    }
}
