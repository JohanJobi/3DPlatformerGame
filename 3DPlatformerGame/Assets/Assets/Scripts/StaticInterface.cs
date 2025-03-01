using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface //this is attatched to the equipment inventory so needs the
                                             //same logic from user interface and needs to inherit some
                                             //methods
{
    public GameObject[] slots;

    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>(); // contains information on which game
                                                                        // object is in which equipment slot

        //link database and inventory items
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = slots[i];

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); }); //called when entering
                                                                                      //slot
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); }); //called when exiting slot
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); }); //called when starting
                                                                                       //to item from slot
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); }); //called at the end of the
                                                                                   //drag
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); }); //called when dragging object

            inventory.GetSlots[i].slotDisplay = obj; //updates display for inventory slot
            slotsOnInterface.Add(obj, inventory.GetSlots[i]); // adds the slot and its inventory slot
                                                              // to the dictionary
        }
        
    }
}
