using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NumberGameController : MonoBehaviour
{
    public GameObject parkourButton;
    public TextMeshPro displayText;
    public TextMeshPro displayText2;
    private int targetNumber;

    public bool player1OnButton;
    public bool player2OnButton;

    public AudioClip ringSound; 
    private AudioSource audioSource;

    public int player1Number;
    public int player2Number;

    public int attempts;

    void Start()
    {
        attempts = 0;
        SpawnTargetNumber();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = ringSound;
    }

    void Update()
    {

        // Check if both players are standing on buttons
        if (player1OnButton && player2OnButton)
        {
            int sum = player1Number + player2Number;
            // Check if the sum matches the target number
            if (sum == targetNumber)
            {
                displayText.color = Color.green;
                displayText2.color = Color.green;
                audioSource.Play();
                StartNewRound();
            }
        }
    }

    void StartNewRound()
    {
        attempts++;

        if (attempts <= 5)
        {
            SpawnTargetNumber();
        }
        else
        {
            parkourButton.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    void SpawnTargetNumber()
    {

        targetNumber = Random.Range(2, 19);
        displayText.color = Color.white;
        displayText2.color = Color.white;
        displayText.text = targetNumber.ToString();
        displayText2.text = targetNumber.ToString();
    }
}

