using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class WandWeapon : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weaponData;

    [Header("Combat Settings")]
    public Transform shootPoint;
    public LayerMask enemyLayer;

    [Header("Mana Settings")]
    public float initialManaCost = 5f;
    private float currentMana = 100f; // geçici mana
    private bool isFiring = false;
    private float manaConsumptionTimer = 0f;

    [Header("DOTween Settings")]
    public float punchStrength = 0.2f;
    public float punchDuration = 0.1f;
    public int punchVibrato = 10;
    public float punchElasticity = 1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartFiring();
        }

        if (Input.GetMouseButton(0) && isFiring)
        {
            ContinueFiring();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopFiring();
        }
    }

    void StartFiring()
    {
        if (currentMana < initialManaCost)
        {
            Debug.Log("not enough mana to start firing!");
            return;
        }

        isFiring = true;
        manaConsumptionTimer = 0f;
        currentMana -= initialManaCost;

        // ateþ animasyonu
        transform.DOPunchPosition(-transform.forward * punchStrength, punchDuration, punchVibrato, punchElasticity);

        Debug.Log("firing started initial mana consumed: " + initialManaCost);
    }

    void ContinueFiring()
    {
        manaConsumptionTimer += Time.deltaTime;

        if (manaConsumptionTimer >= 1f)
        {
            manaConsumptionTimer = 0f;

            if (currentMana >= weaponData.manaCostPerSecond)
            {
                currentMana -= weaponData.manaCostPerSecond;
                ApplyDamage();
                Debug.Log("continuing firing mana consumed: " + weaponData.manaCostPerSecond);
            }
            else
            {
                Debug.Log("no mana anymore!");
                StopFiring();
            }
        }
    }

    void StopFiring()
    {
        isFiring = false;
        Debug.Log("firing stopped.");
    }

    void ApplyDamage()
    {
        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponData.range, enemyLayer))
        {
            EnemyTest enemy = hit.transform.GetComponent<EnemyTest>();

            if (enemy != null)
            {
                enemy.TakeDamage(weaponData.damage);
                Debug.Log(enemy.name + " damaged: " + weaponData.damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (shootPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(shootPoint.position, shootPoint.position + shootPoint.forward * weaponData.range);
        }
    }
}
