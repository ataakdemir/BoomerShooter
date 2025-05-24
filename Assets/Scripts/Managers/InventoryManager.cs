using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Weapons")]
    public WeaponData defaultWeapon;
    public WeaponData eqippedWeapon;

    [Header("Weapon Holders")]
    public Transform weaponHolder;

    private GameObject currentWeaponObject;

    void Start()
    {
        EquipWeapon(defaultWeapon);
    }
    public void EquipWeapon(WeaponData newWeaponData)
    {
        eqippedWeapon = newWeaponData;

    }
    private void ChangeWeaponPrefab(WeaponData WeaponData)
    {

    }
    void Update()
    {

    }
}
