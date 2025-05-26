using System.Collections;
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

    [Header("Visual Bullet Settings")]
    public GameObject bulletVisualPrefab; // 👈 Görsel mermi prefabı
    public float bulletVisualSpeed = 50f; // 👈 Görsel mermi hızı

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

        // 🔴 Görsel mermi spawn
        Vector3 targetPoint = shootPoint.position + shootPoint.forward * weaponData.range;
        GameObject bullet = Instantiate(bulletVisualPrefab, shootPoint.position, Quaternion.identity);
        bullet.AddComponent<BulletVisual>().Initialize(targetPoint, bulletVisualSpeed);

        // 🔴 Raycast hasar
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

    private class BulletVisual : MonoBehaviour
    {
        private Vector3 target;
        private float speed;

        public void Initialize(Vector3 _target, float _speed)
        {
            target = _target;
            speed = _speed;
            Destroy(gameObject, 1f);
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.1f)
                Destroy(gameObject);
        }
    }
}
