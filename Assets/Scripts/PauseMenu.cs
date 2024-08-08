using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Vector3 savedPosition;
    public GameObject pauseMenuUIP; // The UI panel that appears when the game is paused
    private bool isPaused = false;

    void Start()
    {
       
    }
    public void SavePlayerState()
    {
        savedPosition = transform.position;
    }

    public void LoadPlayerState()
    {
        transform.position = savedPosition;
    }

    public void Pause()
    {
        Debug.Log("Pause called");
        pauseMenuUIP.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        SavePlayerState();
    }

    public void Resume()
    {
        Debug.Log("Resume called");
        pauseMenuUIP.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        LoadPlayerState();
    }

    public void OnPauseButtonPressed()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void OnContinueButtonPressed()
    {
        Resume();
    }
}
