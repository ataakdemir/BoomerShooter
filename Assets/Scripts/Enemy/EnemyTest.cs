using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(gameObject.name + " hasar aldý: " + amount + " | Kalan can: " + health);

        if (health <= 0)
            Destroy(gameObject);
    }
}
