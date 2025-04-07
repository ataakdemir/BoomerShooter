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

        // Raycast yaparak en yakýn düþmaný bul:
        RaycastHit hit;
        Vector3 targetPosition;

        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponData.range))
        {
            targetPosition = hit.point;
            Debug.DrawLine(shootPoint.position, targetPosition, Color.green, 1f);
        }
        else
        {
            // Eðer düþman yoksa maksimum menzildeki noktaya ateþ et
            targetPosition = shootPoint.position + shootPoint.forward * weaponData.range;
            Debug.DrawLine(shootPoint.position, targetPosition, Color.red, 1f);
        }

        // Bu noktadan etki alaný oluþtur ve düþmanlara hasar ver:
        Collider[] hitEnemies = Physics.OverlapSphere(targetPosition, impactRadius);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyTest enemyScript = enemy.GetComponent<EnemyTest>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(weaponData.damage);
                Debug.Log(enemy.name + " damaged: " + weaponData.damage);
            }
        }

        // Etki alanýný görsel olarak görmek için:
        Debug.DrawRay(targetPosition, Vector3.up * 2f, Color.yellow, 1f);
    }

    void OnDrawGizmosSelected()
    {
        if (shootPoint == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(shootPoint.position + shootPoint.forward * weaponData.range, 3.5f);
    }
}
