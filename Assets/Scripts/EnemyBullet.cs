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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Çarpışma gerçekleşti: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            Movement player = collision.gameObject.GetComponent<Movement>();
            if (player != null)
            {
                player.PlayerTakesDamage(damage);
            }

            Destroy(gameObject); // ✅ Oyuncuya çarptıktan sonra mermiyi yok et
        }

        // ✅ Obstacle'a çarptıysa da mermiyi yok et
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
