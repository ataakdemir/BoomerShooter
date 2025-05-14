using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public float bonusHealth = 30f;

    public void Update()
    {
        transform.Rotate(0f, 150f * Time.deltaTime, 0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Movement movement = other.GetComponent<Movement>();

            if(movement != null)
            {
                movement.HealPlayer(bonusHealth);
                Destroy(gameObject);
            }
        }
    }

}
