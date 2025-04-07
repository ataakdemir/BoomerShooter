using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void StartMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
