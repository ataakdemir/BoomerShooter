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
    public float currentMana = 100f;         // EKLENDÝ (private yerine public ya da protected de olabilir)
    public float maxMana = 100f;             // EKLENDÝ (Maksimum mana)
    public float manaRegenAmount = 2f;       // EKLENDÝ (Her saniye artacak mana miktarý)
    private bool isFiring = false;
    private float manaConsumptionTimer = 0f;


    [Header("DOTween Settings")]
    public float punchStrength = 0.2f;
    public float punchDuration = 0.1f;
    public int punchVibrato = 10;
    public float punchElasticity = 1f;

    private void Start()
    {
        StartCoroutine(RegenManaOverTime());
    }
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

        transform.DOPunchPosition(-transform.forward * punchStrength, punchDuration, punchVibrato, punchElasticity);

        ApplyDamage();

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

                transform.DOPunchPosition(-transform.forward * punchStrength, punchDuration, punchVibrato, punchElasticity);

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

        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponData.range))
        {
            Debug.Log(hit.transform.name + " hit by Wand");

            EnemyTest enemy = hit.transform.GetComponent<EnemyTest>();
            if (enemy != null)
            {
                enemy.TakeDamage(weaponData.damage);
                Debug.Log(enemy.name + " damaged: " + weaponData.damage);
            }
        }
        else
        {
            Debug.Log("Nothing hit by raycast");
        }
    }

    IEnumerator RegenManaOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);  // Her 1 saniyede bir artýþ
            currentMana += manaRegenAmount;

            if (currentMana > maxMana)           // Eðer maxMana'yý aþarsa...
            {
                currentMana = maxMana;
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
