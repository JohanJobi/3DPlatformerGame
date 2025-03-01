using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMusic : MonoBehaviour
{
    public static backgroundMusic instance; 

    public AudioSource music;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject); //so the music doesnt stop playing
    }

    void Start()
    {
        // Play the background music when the game starts
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        music.Play();
    }
}


