using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public float health = 50f;
    private static int currentEnemyDeathSoundIndex = 0;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(gameObject.name + " hasar aldý: " + amount + " | Kalan can: " + health);

        if (health <= 0)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyDeathSounds[currentEnemyDeathSoundIndex]);
        currentEnemyDeathSoundIndex = (currentEnemyDeathSoundIndex + 1) % AudioManager.Instance.enemyDeathSounds.Length;
        Destroy(gameObject);
    }
}
