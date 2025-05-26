using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Ending : MonoBehaviour
{
    public float delay;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {            
                StartCoroutine(LoadWinSceneAfterDelay());
        }
    }
    IEnumerator LoadWinSceneAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("WinScreen");
    }
}
