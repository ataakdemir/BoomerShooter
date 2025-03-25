using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BowWeapon : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weaponData;

    [Header("Combat Settings")]
    public Transform shootPoint;
    public LayerMask enemyLayer;

    private float currentMana = 100f;
    private float chargeStartTime;
    private bool isCharging = false;

    [Header("DOTween Animation Settings")]
    public float punchStrength = 0.2f;
    public float punchDuration = 0.15f;
    public int punchVibrato = 10;
    public float punchElasticity = 1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCharge();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseShot();
        }
    }

    void StartCharge()
    {
        isCharging = true;
        chargeStartTime = Time.time;
        Debug.Log("charcging ...");
    }

    void ReleaseShot()
    {
        if (!isCharging)
            return;

        isCharging = false;
        float chargeDuration = Time.time - chargeStartTime;

        float manaCost;
        float impactRadius;

        if (chargeDuration >= 2f)
        {
            manaCost = 6f;
            impactRadius = 3.5f;
            Debug.Log("full of charge");
        }
        else if (chargeDuration >= 1f)
        {
            manaCost = 4f;
            impactRadius = 2.5f;
            Debug.Log("mid charge");
        }
        else
        {
            manaCost = 2f;
            impactRadius = 1.5f;
            Debug.Log("no charge");
        }

        if (currentMana < manaCost)
        {
            Debug.Log("no mana anymore");
            return;
        }

        currentMana -= manaCost;

        transform.DOPunchPosition(-transform.forward * punchStrength, punchDuration, punchVibrato, punchElasticity);

        // alan hasarý
        Collider[] hitEnemies = Physics.OverlapSphere(shootPoint.position + shootPoint.forward * weaponData.range, impactRadius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyTest enemyScript = enemy.GetComponent<EnemyTest>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(weaponData.damage);
                Debug.Log(enemy.name + " damaged: " + weaponData.damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (shootPoint == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(shootPoint.position + shootPoint.forward * weaponData.range, 3.5f);
    }
}
