using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NailGunWeapon : MonoBehaviour
{

    [Header("Shake Settings")]
    public float shakeDuration = 0.1f;
    public Vector3 shakeStrength = new Vector3(2f, 0f, 0f);
    public int shakeVibrato = 10;
    public float shakeRandomness = 90f;

    [Header("Weapon Data")]
    public WeaponData weaponData;

    [Header("Bullet Settings")]
    public int magazineSize = 30;
    public float reloadTime = 1.5f;
    public float fireRate = 0.1f; // seri atýþlar için
    public Transform shootPoint;

    private int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    private float currentMana = 100f; //geçici mana
    void Start()
    {
        currentAmmo = magazineSize;
    }

    void Update()
    {
        if (isReloading)
            return;

        // reload
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (currentMana < weaponData.manaCost)
        {
            Debug.Log("no mana anymore");
            return;
        }

        currentAmmo--;
        currentMana -= weaponData.manaCost;
       
        transform.DOShakeRotation(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness);
        
        Debug.DrawRay(shootPoint.position, shootPoint.forward * weaponData.range, Color.red, 1f);

        // RAYCAST
        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponData.range))
        {
            Debug.Log(hit.transform.name + " you shot the enemy ");

            EnemyTest enemy = hit.transform.GetComponent<EnemyTest>();
            if (enemy != null)
            {
                enemy.TakeDamage(weaponData.damage);
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log(" reloading ");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;
        isReloading = false;

        Debug.Log(" reloaded ");
    }

    // yön için
    private void OnDrawGizmosSelected()
    {
        if (shootPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(shootPoint.position, shootPoint.position + shootPoint.forward * weaponData.range);
        }
    }
}
