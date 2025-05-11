using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPitDmg : MonoBehaviour
{
    public Transform Player;
    
    void OnTriggerEnter(Collider other)
    {
        Player.GetComponent<Movement>().Die();
    }
}
