using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    public string levelToPlay;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelToPlay);
    }
    public void StartMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Alternative StartMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void HellMode()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HellMode");
    }

    public void SecondLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelEditorScene");
    }
}
