using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Database Item", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] ItemObjects;
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>(); //gives and ID to every
                                                                                    //item using a dictionary


    public void OnAfterDeserialize()
    {// calls after the scritable object has been deserialized

        for (int i = 0; i < ItemObjects.Length; i++)
        {
            //iterates through item objects and gives them an ID
            //adds item object to the get item dictionary using the index as the key
            ItemObjects[i].data.Id = i;
            GetItem.Add(i, ItemObjects[i]);
           
        }
    }

    public void OnBeforeSerialize()
    {
        //clears the dictionary before serialization
        //this makes sure that data added during the onafterdesrialize method 
        //this prevents issues with duplicate keys 
        GetItem = new Dictionary<int, ItemObject>();
    }
}
