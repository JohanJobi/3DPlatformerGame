using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberOnButton : MonoBehaviour
{
    public int Number;
    public bool playerOnButton;
    public NumberGameController gameController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            gameController.player2OnButton = true;
            gameController.player2Number = Number;
        }

        if (other.CompareTag("Player"))
        {
            gameController.player1OnButton = true;
            gameController.player1Number = Number;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            gameController.player2OnButton = false;
            gameController.player2Number = 0;
        }

        if (other.CompareTag("Player"))
        {
            gameController.player1OnButton = false;
            gameController.player1Number = 0;
        }

    }

}
