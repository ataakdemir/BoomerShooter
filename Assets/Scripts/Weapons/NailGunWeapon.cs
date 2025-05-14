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
    public float fireRate = 0.1f;
    public Transform shootPoint;

    private int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    void Start()
    {
        currentAmmo = magazineSize;
    }

    void Update()
    {
        if (isReloading)
            return;

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
        if (!transform.IsChildOf(GameObject.FindWithTag("Player").GetComponentInChildren<GunInventory>().weaponHolder))
            return;

        if (!PlayerManaManager.Instance.UseMana(weaponData.manaCost))
        {
            Debug.Log("No mana anymore!");
            return;
        }

        currentAmmo--;

        transform.DOShakeRotation(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness);
        
        Debug.DrawRay(shootPoint.position, shootPoint.forward * weaponData.range, Color.red, 1f);

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponData.range))
        {
            EnemyTest enemy = hit.transform.GetComponent<EnemyTest>();
            if (enemy != null)
            {
                enemy.TakeDamage(weaponData.damage);
            }
        }

        AudioManager.Instance.PlaySFX(AudioManager.Instance.nailgunFireSound);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
        isReloading = false;
    }
}