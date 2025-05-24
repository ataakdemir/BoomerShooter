using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public enum WeaponType {Nailgun, Crossbow, Wand, None }
    public WeaponType weaponType;

    public GameObject weaponPrefab;

    private bool isPlayerInRange = false;
    private GameObject player;

    [Header("Weapon Position")]
    public Vector3 weaponLocalPosition = Vector3.zero;
    public Vector3 weaponLocalRotation = Vector3.zero;

    void Start()
    {
        WeaponManager.Instance.RegisterPickup(this);
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            GunInventory inventory = player.GetComponentInChildren<GunInventory>();
            if (inventory != null)
            {
                WeaponType oldWeapon = inventory.GetCurrentRangedWeaponType();
                inventory.SetNewRangedWeapon(weaponPrefab, weaponType, weaponLocalPosition, weaponLocalRotation);

                isPlayerInRange = false;
                player = null;
                gameObject.SetActive(false);

                if (oldWeapon != WeaponType.None)
                    WeaponManager.Instance.ReactivatePickup(oldWeapon);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;
        }
    }

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }
}
