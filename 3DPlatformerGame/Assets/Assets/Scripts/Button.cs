using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject parkour;
    private bool activated;


    private void Start()
    {
        parkour.SetActive(false);//parkour initially deactivated
    }

    private void OnTriggerEnter(Collider collision)
    {

       
        if (collision.CompareTag("Player"))
        {
            parkour.SetActive(true); //if the collision is a player then the parkour is set to active
        }
    }


    private void OnTriggerExit(Collider other)
    {

        //when player exits the button it will disable the parkour course
        if (other.CompareTag("Player"))
        {
            parkour.SetActive(false);
        }
    }
}
