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
    public float manaCostPerSecond = 5f;

    [Header("DOTween Settings")]
    public float punchStrength = 0.2f;
    public float punchDuration = 0.1f;
    public int punchVibrato = 10;
    public float punchElasticity = 1f;

    private bool isFiring = false;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!transform.IsChildOf(GameObject.FindWithTag("Player").GetComponentInChildren<GunInventory>().weaponHolder))
            return;

        if (Input.GetMouseButtonDown(0))
            StartFiring();

        if (Input.GetMouseButton(0) && isFiring)
            ContinueFiring();

        if (Input.GetMouseButtonUp(0))
            StopFiring();
    }

    void StartFiring()
    {
        if (!gameObject.activeInHierarchy) return;

        if (!PlayerManaManager.Instance.UseMana(initialManaCost))
        {
            Debug.Log("Not enough mana to start firing!");
            return;
        }

        isFiring = true;
        lineRenderer.enabled = true;
        transform.DOPunchPosition(-transform.forward * punchStrength, punchDuration, punchVibrato, punchElasticity);
    }

    void ContinueFiring()
    {
        if (!gameObject.activeInHierarchy) return;

        if (!PlayerManaManager.Instance.UseMana(manaCostPerSecond * Time.deltaTime))
        {
            Debug.Log("No mana anymore!");
            StopFiring();
            return;
        }

        ApplyDamage();
        UpdateBeamVisual();
    }

    void StopFiring()
    {
        isFiring = false;
        lineRenderer.enabled = false;
    }

    void ApplyDamage()
    {
        Debug.DrawRay(shootPoint.position, shootPoint.forward * weaponData.range, Color.cyan);

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponData.range))
        {
            EnemyTest enemy = hit.transform.GetComponent<EnemyTest>();
            if (enemy != null)
                enemy.TakeDamage(weaponData.damage * Time.deltaTime);
        }
    }

    void UpdateBeamVisual()
    {
        Vector3 start = shootPoint.position;
        Vector3 end = shootPoint.position + shootPoint.forward * weaponData.range;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
