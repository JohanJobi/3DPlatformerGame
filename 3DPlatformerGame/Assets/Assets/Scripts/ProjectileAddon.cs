using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    public int damage = 5;
    private Rigidbody rb;
    private float instantiateTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        instantiateTime = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Enemy1 enemy1 = other.GetComponent<Enemy1>();
        if (enemy1 != null)
        {
            enemy1.TakeDamage(damage);

        }

        // make sure projectile sticks to surface
        rb.isKinematic = true;
        // make sure projectile moves with target
        transform.SetParent(other.transform);
    }

    private void Update()
    {
        if (Time.time - instantiateTime > 5f)
        {
            Destroy(gameObject);
        }
    }
}
