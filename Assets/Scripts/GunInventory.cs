using UnityEngine;

public class GunInventory : MonoBehaviour
{
    public GameObject meleeWeapon;
    public GameObject rangedWeapon;
    public Transform weaponHolder;

    private WeaponPickUp.WeaponType currentRangedWeaponType;

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
        if (meleeWeapon != null)
            meleeWeapon.SetActive(isMeleeWeaponSelected);

        if (rangedWeapon != null)
            rangedWeapon.SetActive(!isMeleeWeaponSelected);
    }

    public void SetNewRangedWeapon(GameObject newWeaponPrefab, WeaponPickUp.WeaponType newType, Vector3 position, Vector3 rotation)
    {
        if (rangedWeapon != null)
            Destroy(rangedWeapon);

        rangedWeapon = Instantiate(newWeaponPrefab, weaponHolder);
        rangedWeapon.transform.localPosition = position;
        rangedWeapon.transform.localRotation = Quaternion.Euler(rotation);

        currentRangedWeaponType = newType;

        rangedWeapon.SetActive(!isMeleeWeaponSelected);
    }

    public WeaponPickUp.WeaponType GetCurrentRangedWeaponType()
    {
        return currentRangedWeaponType;
    }
}
