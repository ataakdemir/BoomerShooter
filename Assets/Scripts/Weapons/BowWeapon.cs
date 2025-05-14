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
            StartCharge();

        if (Input.GetMouseButtonUp(0))
            ReleaseShot();
    }

    void StartCharge()
    {
        isCharging = true;
        chargeStartTime = Time.time;
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
        }
        else if (chargeDuration >= 1f)
        {
            manaCost = 4f;
            impactRadius = 2.5f;
        }
        else
        {
            manaCost = 2f;
            impactRadius = 1.5f;
        }

        if (!PlayerManaManager.Instance.UseMana(manaCost))
        {
            Debug.Log("No mana anymore!");
            return;
        }

        transform.DOPunchPosition(-transform.forward * punchStrength, punchDuration, punchVibrato, punchElasticity);

        RaycastHit hit;
        Vector3 targetPosition = shootPoint.position + shootPoint.forward * weaponData.range;

        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponData.range))
        {
            targetPosition = hit.point;
        }

        Debug.DrawLine(shootPoint.position, targetPosition, Color.green, 1f);
        Debug.DrawRay(targetPosition, Vector3.up * impactRadius, Color.yellow, 1f);

        Collider[] hitEnemies = Physics.OverlapSphere(targetPosition, impactRadius);
        foreach (Collider enemy in hitEnemies)
        {
            EnemyTest enemyScript = enemy.GetComponent<EnemyTest>();
            if (enemyScript != null)
                enemyScript.TakeDamage(weaponData.damage);
        }
    }
}
