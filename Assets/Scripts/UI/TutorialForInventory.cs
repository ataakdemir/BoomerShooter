using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialForInventory : MonoBehaviour
{
    public GameObject tutorial;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorial != null)
                tutorial.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorial != null)
                tutorial.SetActive(false);
        }
    }
}
