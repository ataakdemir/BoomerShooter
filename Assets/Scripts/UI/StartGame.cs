using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    public string levelToLoad;
    public void Play()
    {
        SceneManager.LoadScene(levelToLoad);
    } 

    public void Quit()
    {
        Application.Quit();
    }
}
