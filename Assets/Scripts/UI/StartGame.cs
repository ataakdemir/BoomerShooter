using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    public string levelToPlay;
    public void Play()
    {
        SceneManager.LoadScene(levelToPlay);
    } 

    public void Quit()
    {
        Application.Quit();
    }
    public void HellMode()
    {
        SceneManager.LoadScene("HellMode");
    }
}
