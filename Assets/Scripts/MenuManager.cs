using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    private ScoreSystem scoreSystem;

    void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
    }
    public void MoveToScene (int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown (KeyCode.Escape))
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
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive (true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}
