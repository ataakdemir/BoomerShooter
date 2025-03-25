using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PipeWeapon : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weaponData;

    [Header("Combat Settings")]
    public Transform attackPoint;
    public float attackRadius = 0.5f;  //etki alaný yarýçapý
    public LayerMask enemyLayer;
    private float currentMana = 100f;
   
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
    void Attack()
    {
        if (currentMana < weaponData.manaCost)
        {
            Debug.Log("Yeterli mana yok!");
            return;
        }

        currentMana -= weaponData.manaCost;

       

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, weaponData.range);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyTest enemyScript = enemy.GetComponent<EnemyTest>();

            if (enemyScript != null)
            {
                enemyScript.TakeDamage(weaponData.damage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, weaponData.range);
    }

}
