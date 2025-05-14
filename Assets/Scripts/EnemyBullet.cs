using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage;
    public float lifetime;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Movement player = other.GetComponent<Movement>();
            if (player != null)
            {
                player.PlayerTakesDamage(damage);
            }
        }

        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }

}
