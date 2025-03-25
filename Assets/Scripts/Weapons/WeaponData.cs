using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("General Info")]
    public string weaponName;
    public WeaponType weaponType;

    [Header("Combat Info")]
    public float damage;
    public float range;
    public float manaCost; //mana tüketimi (tek seferlik atýþlar için)

    [Header("Continuous Fire Settings (Only for Magic Staff)")]
    public bool isContinuous; //surekli ates ediyo mu
    public float manaCostPerSecond; //sn de tüketilen mana
    //bow için charge olayýný bow scriptinde yazýyom

    [Header("Prefab")]
    public GameObject weaponPrefab; //prefab silah
}

public enum WeaponType
{
    Melee,
    Ranged,
    Magic
}