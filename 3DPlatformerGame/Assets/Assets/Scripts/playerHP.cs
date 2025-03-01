using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerHP : MonoBehaviour
{
    public int health = 100;
    public healthBar healthBar;
    public PlayerInventory inventory;
    public bool dead;
    private void Start()
    {
        healthBar.setMaxHealth(health);
    }
    public void TakeDamage(int damage)
    {
        if (inventory.helmetHealth > 0 || inventory.chestHealth > 0 || inventory.bootsHealth > 0)
        {//takes damage off armour items first
            if(inventory.helmetHealth > 0)
            {
                inventory.helmetHealth -= damage;
                if(inventory.helmetHealth <= 0)
                {
                    inventory.RemoveItem(inventory.helmet);
                }
                return;   
            }            
            if(inventory.chestHealth > 0)
            {
                inventory.chestHealth -= damage;
                if (inventory.chestHealth <= 0)
                {
                    inventory.RemoveItem(inventory.chest);
                }
                return;
            }            
            if(inventory.bootsHealth > 0 )
            {
                inventory.bootsHealth -= damage;
                if (inventory.bootsHealth <= 0)
                {
                    inventory.RemoveItem(inventory.boots);
                }
                return;
            }
        }
        else if (health > 0)
        {
            health -= damage;
            healthBar.setHealth(health);
        }
        else
        {
            Destroy(gameObject);
            dead = true;
        }
    }
}
