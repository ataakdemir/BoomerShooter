using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PipeWeapon : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weaponData;

    [Header("Combat Settings")]
    public Transform attackPoint;

    [Header("DOTween Punch Settings")]
    public Vector3 punchRotation = new Vector3(0f, 0f, 70f);
    public float punchDuration = 0.2f;
    public int punchVibrato = 8;
    public float punchElasticity = 1f;

    [Header("Cooldown Settings")]
    public float attackCooldown = 0.3f;
    private bool canAttack = true;
    private Quaternion originalRotation;

    private static int currentPipeSoundIndex = 0;

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
            Attack();
    }

    void Attack()
    {
        if (!PlayerManaManager.Instance.UseMana(weaponData.manaCost))
        {
            Debug.Log("Yeterli mana yok!");
            return;
        }

        canAttack = false;
        transform.DOPunchRotation(punchRotation, punchDuration, punchVibrato, punchElasticity)
            .OnComplete(() => transform.localRotation = originalRotation);

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, weaponData.range);
        foreach (Collider enemy in hitEnemies)
        {
            EnemyTest enemyScript = enemy.GetComponent<EnemyTest>();
            if (enemyScript != null)
                enemyScript.TakeDamage(weaponData.damage);
        }
        StartCoroutine(ResetAttackCooldown());

        AudioManager.Instance.PlaySFX(AudioManager.Instance.pipeHitSounds[currentPipeSoundIndex]);
        currentPipeSoundIndex = (currentPipeSoundIndex + 1) % AudioManager.Instance.pipeHitSounds.Length;
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
