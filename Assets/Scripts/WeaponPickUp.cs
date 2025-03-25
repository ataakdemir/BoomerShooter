using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public GameObject weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GunInventory inventory = other.GetComponentInChildren<GunInventory>();
            if (inventory != null)
            {
                inventory.SetNewRangedWeapon(weaponPrefab);
                Destroy(gameObject); 
            }
        }
    }
}
