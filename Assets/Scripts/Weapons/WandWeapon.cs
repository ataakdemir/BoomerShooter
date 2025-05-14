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

    [Header("Mana Settings")]
    public float initialManaCost = 5f;

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
            StartFiring();

        if (Input.GetMouseButton(0) && isFiring)
            ContinueFiring();

        if (Input.GetMouseButtonUp(0))
            StopFiring();
    }

    void StartFiring()
    {
        if (!PlayerManaManager.Instance.UseMana(initialManaCost))
        {
            Debug.Log("Not enough mana to start firing!");
            return;
        }

        isFiring = true;
        manaConsumptionTimer = 0f;
        ApplyDamage();
        transform.DOPunchPosition(-transform.forward * punchStrength, punchDuration, punchVibrato, punchElasticity);
    }

    void ContinueFiring()
    {
        manaConsumptionTimer += Time.deltaTime;

        if (manaConsumptionTimer >= 1f)
        {
            manaConsumptionTimer = 0f;

            if (!PlayerManaManager.Instance.UseMana(weaponData.manaCostPerSecond))
            {
                Debug.Log("No mana anymore!");
                StopFiring();
                return;
            }

            ApplyDamage();
            transform.DOPunchPosition(-transform.forward * punchStrength, punchDuration, punchVibrato, punchElasticity);
        }
    }

    void StopFiring()
    {
        isFiring = false;
    }

    void ApplyDamage()
    {
        RaycastHit hit;
        Debug.DrawRay(shootPoint.position, shootPoint.forward * weaponData.range, Color.cyan, 1f);

        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponData.range))
        {
            EnemyTest enemy = hit.transform.GetComponent<EnemyTest>();
            if (enemy != null)
                enemy.TakeDamage(weaponData.damage);
        }
    }
}