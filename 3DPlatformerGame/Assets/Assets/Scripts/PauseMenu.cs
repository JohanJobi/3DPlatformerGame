using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; //handles namespace within the unity scene view 

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private void Start()
    {
        Resume();
    }

    private bool wasPKeyPressed = false; //vairable to track whether P was pressed
    void Update()
    {
        if (Input.GetKey(KeyCode.P) && !wasPKeyPressed) //only whe P is pressed 
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        //Update the was pressed key for the next frame
        wasPKeyPressed = Input.GetKey(KeyCode.P);

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); //sets booloean parameters of menu UI to be true
        Time.timeScale = 0f; //Pauses the time of the game 
        GameIsPaused = true; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); //changes scene to menu
    }
}
