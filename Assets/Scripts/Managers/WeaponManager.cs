using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    private Dictionary<WeaponPickUp.WeaponType, WeaponPickUp> pickups = new Dictionary<WeaponPickUp.WeaponType, WeaponPickUp>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterPickup(WeaponPickUp pickup)
    {
        WeaponPickUp.WeaponType type = pickup.GetWeaponType();
        if (!pickups.ContainsKey(type))
        {
            pickups.Add(type, pickup);
        }
    }

    public void ReactivatePickup(WeaponPickUp.WeaponType type)
    {
        if (pickups.ContainsKey(type))
        {
            WeaponPickUp pickup = pickups[type];
            if (pickup != null && !pickup.gameObject.activeSelf)
            {
                pickup.gameObject.SetActive(true);
            }
        }
    }
}