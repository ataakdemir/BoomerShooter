using UnityEngine;
using DG.Tweening;

public class BowWeapon : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weaponData;

    [Header("Combat Settings")]
    public Transform shootPoint;

    [Header("DOTween Animation Settings")]
    public float punchStrength = 0.2f;
    public float punchDuration = 0.15f;
    public int punchVibrato = 10;
    public float punchElasticity = 1f;

    [Header("Visual Bullet Settings")]
    public GameObject bulletVisualPrefab;
    public float bulletVisualSpeed = 30f;

    private bool isFiring = false;

    void Update()
    {
        if (!transform.IsChildOf(GameObject.FindWithTag("Player").GetComponentInChildren<GunInventory>().weaponHolder))
            return;

        if (Input.GetMouseButtonDown(0) && !isFiring)
            StartCoroutine(ShootWithDelay());
    }

    System.Collections.IEnumerator ShootWithDelay()
    {
        isFiring = true;

        yield return new WaitForSeconds(0.2f); // 0.2 saniye gecikme

        // Mana kontrolü
        float manaCost = 6f;
        if (!PlayerManaManager.Instance.UseMana(manaCost))
        {
            Debug.Log("No mana anymore!");
            isFiring = false;
            yield break;
        }

        transform.DOPunchPosition(-transform.forward * punchStrength, punchDuration, punchVibrato, punchElasticity);

        Vector3 centerDir = shootPoint.forward;
        Vector3 rightDir = Quaternion.Euler(0, 10, 0) * centerDir;
        Vector3 leftDir = Quaternion.Euler(0, -10, 0) * centerDir;

        CreateBullet(centerDir);
        CreateBullet(rightDir);
        CreateBullet(leftDir);

        AudioManager.Instance.PlaySFX(AudioManager.Instance.bowFireSound);

        isFiring = false;
    }

    void CreateBullet(Vector3 direction)
    {
        Vector3 targetPosition = shootPoint.position + direction * weaponData.range;

        GameObject bullet = Instantiate(bulletVisualPrefab, shootPoint.position, Quaternion.identity);
        bullet.AddComponent<BulletVisual>().Initialize(targetPosition, bulletVisualSpeed, direction, weaponData.range, weaponData.damage);

        Debug.DrawLine(shootPoint.position, targetPosition, Color.green, 1f);
    }

    private class BulletVisual : MonoBehaviour
    {
        private Vector3 target;
        private float speed;
        private Vector3 direction;
        private float range;
        private float damage;

        private Vector3 startPosition;

        public void Initialize(Vector3 _target, float _speed, Vector3 _direction, float _range, float _damage)
        {
            target = _target;
            speed = _speed;
            direction = _direction;
            range = _range;
            damage = _damage;
            startPosition = transform.position;

            Destroy(gameObject, 1.5f); 
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            Vector3 lookDir = (target - transform.position).normalized;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(lookDir);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, speed * Time.deltaTime))
            {
                EnemyTest enemyScript = hit.transform.GetComponent<EnemyTest>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(damage);
                    Destroy(gameObject); 
                }
            }

            if (Vector3.Distance(startPosition, transform.position) >= range)
                Destroy(gameObject);
        }
    }
}
