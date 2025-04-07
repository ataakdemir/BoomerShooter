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
    public Vector3 punchRotation = new Vector3(0f, 0f, 0f); // Dönüþ miktarý
    public float punchDuration = 0.2f;                       // Animasyon süresi
    public int punchVibrato = 8;                             // Titreme sayýsý
    public float punchElasticity = 1f;                       // Esneklik oraný

    public Camera playerCamera;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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

        currentMana -= weaponData.manaCost;
        Vector3 punchDir = playerCamera.transform.forward;
        transform.DOPunchPosition(punchDir.normalized * 0.2f, punchDuration, punchVibrato, punchElasticity);

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, weaponData.range);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyTest enemyScript = enemy.GetComponent<EnemyTest>();

            if (enemyScript != null)
            {
                enemyScript.TakeDamage(weaponData.damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, weaponData.range);
    }
}
