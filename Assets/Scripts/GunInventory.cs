using UnityEngine;

public class GunInventory : MonoBehaviour
{
    public GameObject meleeWeapon;
    public GameObject rangedWeapon;
    public Transform weaponHolder;

    private bool isMeleeWeaponSelected = true;

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isMeleeWeaponSelected = true;
            SelectWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isMeleeWeaponSelected = false;
            SelectWeapon();
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            isMeleeWeaponSelected = !isMeleeWeaponSelected;
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        meleeWeapon.SetActive(isMeleeWeaponSelected);
        if (rangedWeapon != null)
            rangedWeapon.SetActive(!isMeleeWeaponSelected);
    }

    public void SetNewRangedWeapon(GameObject newWeaponPrefab)
    {
        if (rangedWeapon != null)
            Destroy(rangedWeapon);

        rangedWeapon = Instantiate(newWeaponPrefab, weaponHolder);
        rangedWeapon.transform.localPosition = Vector3.zero;
        rangedWeapon.transform.localRotation = Quaternion.identity;

        rangedWeapon.SetActive(!isMeleeWeaponSelected);
    }
}
