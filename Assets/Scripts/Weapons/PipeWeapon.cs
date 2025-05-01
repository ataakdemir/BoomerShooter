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
    public float attackRadius = 0.5f;
    private float currentMana = 100f;

    [Header("DOTween Punch Settings")]
    public Vector3 punchRotation = new Vector3(0f, 0f, 70f); // Dönüþ miktarý
    public float punchDuration = 0.2f;                       // Animasyon süresi
    public int punchVibrato = 8;                             // Titreme sayýsý
    public float punchElasticity = 1f;                       // Esneklik oraný


    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    [Header("Cooldown Settings")]
    public float attackCooldown = 0.3f;
    private bool canAttack = true;



    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (currentMana < weaponData.manaCost)
        {
            Debug.Log("Yeterli mana yok!");
            return;
        }

        canAttack = false;

        currentMana -= weaponData.manaCost;

        transform.DOPunchRotation(punchRotation, punchDuration, punchVibrato, punchElasticity).OnComplete(() => transform.localRotation = originalRotation);

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, weaponData.range);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyTest enemyScript = enemy.GetComponent<EnemyTest>();

            if (enemyScript != null)
            {
                enemyScript.TakeDamage(weaponData.damage);
            }
        }
        StartCoroutine(ResetAttackCooldown());
    }
    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, weaponData.range);
    }
}
