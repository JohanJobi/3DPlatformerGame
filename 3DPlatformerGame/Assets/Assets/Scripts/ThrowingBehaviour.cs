using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowingBehaviour : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public PlayerInventory playerInventory;
    public Item redBalls;


    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;
    private void Start()
    {
        readyToThrow = true;
    }

    private void Update()
    {
        try//code tries checking if there is any redballs in equipment
        {
            totalThrows = playerInventory.equipment.AmountOfItem(redBalls);
        }
        catch (NullReferenceException)//if there is an error then the code does not throw any balls
        {
            return;
        }
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0 && playerInventory.inventoryOpen == false)
        {//conditions for throwing
            Throw();
            playerInventory.TakeAwayItem(redBalls, 1);
        }
        else if (totalThrows == 0)
        {
            try
            {
                playerInventory.RemoveItem(redBalls);
            }
            catch (NullReferenceException)
            {
                return;
            }
        }
    }

    private void Throw()
    {
        
        readyToThrow = false;
        
        // instantiate onject to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direciton using raycast
        Vector3 forceDirection = cam.forward;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f)) 
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }
        //add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
        
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        totalThrows--;

        //implement throw cooldown
        Invoke(nameof(ResetThrow), throwCooldown);


    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }






}
