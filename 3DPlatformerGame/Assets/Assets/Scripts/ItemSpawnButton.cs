using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnButton : MonoBehaviour
{
    public GameObject grapple;
    private void Start()
    {
        grapple.SetActive(false);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player2"))
        {
            grapple.SetActive(true);
        }
    }
}
