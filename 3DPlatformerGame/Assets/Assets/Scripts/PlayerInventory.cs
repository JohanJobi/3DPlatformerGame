using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    public MovementScript movementScript;
    public InventoryObject inventory;
    public InventoryObject equipment;
    public bool inventoryOpen;
    public playerHP player;

    public Item redBall;
    public Item helmet;
    public Item chest;
    public Item boots;
    public bool helmetDetected;
    public bool chestDetected;
    public bool bootsDetected;    
    public int helmetHealth = 0;
    public int chestHealth = 0;
    public int bootsHealth = 0;
    public Item keys;
    public Item food;
    public Item bones;
    public Canvas inventoryDisplay;
    public ProjectileAddon projectileAddon;
    public ThrowingBehaviour script;
    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var item = hit.collider.GetComponent<GroundItem>();
        if (item)
        {
            Item _item = new Item(item.item);
            if(inventory.AddItem(_item, 1))
            {
                Destroy(hit.gameObject);
            }
            
        }
    }
    private void Start()
    {
        bones.Id = 0;
        boots.Id = 1;
        chest.Id = 2;
        food.Id = 3;
        helmet.Id = 5;
        keys.Id = 6;
        redBall.Id = 7;

        inventoryDisplay.enabled = false;
        inventoryOpen = false;
        if (SceneManager.GetActiveScene().name == "Level3")
        {
            inventory.AddItem(redBall, 20);
        }
        

    }
    private void Update()
    {
        //following checks for armour on equipment and and controlls health for each armour piece
        if (InEquipment(chest) && !chestDetected)
        {
            chestHealth += 30;
            chestDetected = true;
        }
        else if (!InEquipment(chest) && chestDetected)
        {
            chestHealth -= 0;
            chestDetected = false;
        }

        if (InEquipment(helmet) && !helmetDetected)
        {
            helmetHealth += 30;
            helmetDetected = true;
        }
        else if (!InEquipment(helmet) && helmetDetected)
        {
            helmetHealth = 0;
            helmetDetected = false;
        }

        if (InEquipment(boots) && !bootsDetected)
        {
            bootsHealth += 30;
            bootsDetected = true;
        }
        else if (!InEquipment(boots) && bootsDetected)
        {
            bootsHealth = 0;
            bootsDetected = false;
        }
        if (inventory.AmountOfItem(keys) >= 5 && SceneManager.GetActiveScene().name == "Level3")
        {//checks amount of keys
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }        
        if (inventory.AmountOfItem(keys) == 1 && SceneManager.GetActiveScene().name == "Level2")
        {
            inventory.Clear();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (InEquipment(food))
        {// controlls food regen
            player.health += AmountInEquipment(food);
            TakeAwayItem(food, AmountInEquipment(food));
            RemoveItem(food);

        }
        if (Input.GetKey(KeyCode.K))
        {//saving
            inventory.Save();
            equipment.Save();

        }
        if (Input.GetKeyDown(KeyCode.L))
        {//loading
            inventory.Load();
            equipment.Load();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {//inventory opening logic
            if(inventoryOpen == true)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0f;
            }
            Cursor.lockState = (Cursor.lockState == CursorLockMode.None) ? CursorLockMode.Locked : CursorLockMode.None;
            //toggles between cursorlockmode.none and cursorlockemode.locked
            Cursor.visible = (Cursor.lockState == CursorLockMode.None);
            //if cursor mode is in none then it makes the cursor visible
            inventoryOpen = !inventoryOpen;
            if (inventoryDisplay != null)
            {
                inventoryDisplay.enabled = !inventoryDisplay.enabled;
            }

            movementScript.enabled = !movementScript.enabled;
        }

    }
    public bool InEquipment(Item Finditem)
    {
        // Loop through the 5 slots in the inventory
        for (int i = 0; i < Mathf.Min(5, equipment.GetSlots.Length); i++)
        {
            InventorySlot currentSlot = equipment.GetSlots[i];
            int itemId = currentSlot.item.Id;
            if (itemId == Finditem.Id)
            {
                return true;
            }
        }
        return false;
    }
     public int AmountInEquipment(Item item)
    {
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            InventorySlot currentSlot = equipment.GetSlots[i];

            int itemId = currentSlot.item.Id;
            if (itemId == item.Id)
            {
                return currentSlot.amount;
            }
        }
        return 0;
    }
    public void TakeAwayItem(Item item, int amount)
    {
        equipment.AddItem(item, -amount);
    }

    public void RemoveItem(Item item)
    {
        InventorySlot slot = equipment.FindItemOnInventory(item);
        slot.RemoveItem();
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
        equipment.Container.Clear();
    }
}
