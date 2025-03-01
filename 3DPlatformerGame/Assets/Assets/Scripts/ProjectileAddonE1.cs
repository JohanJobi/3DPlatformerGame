using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileAddonE1 : MonoBehaviour
{
    public int damage = 5;
    private float instantiateTime;

    private void Start()
    {
        instantiateTime = Time.time;
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an object named "player1"
        if (collision.gameObject.name == "player1")
        {
            playerHP playerHP = collision.gameObject.GetComponent<playerHP>();
            playerHP.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
    private void Update()
    {
        if(Time.time - instantiateTime> 5f)
        {
            Destroy(gameObject);
        }
    }

}

