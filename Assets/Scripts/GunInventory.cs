using UnityEngine;

public class GunInventory : MonoBehaviour
{
    [Header("Assigned Weapons")]
    public GameObject meleeWeapon;
    public GameObject rangedWeapon;
    public Transform weaponHolder;

    // Envanterde şu anda hangi "ranged" silahın tutulduğunu takip eder.
    // Başlangıçta None olarak ayarlanıyor.
    private WeaponPickUp.WeaponType currentRangedWeaponType = WeaponPickUp.WeaponType.None;

    // 1 tuşu ile melee, 2 tuşu ile ranged silah seçimine geçebilmek için
    private bool isMeleeWeaponSelected = true;

    void Start()
    {
        // Oyuna başladığında ilk silah seçimini uygula
        SelectWeapon();
    }

    void Update()
    {
        // Tuşla silah değişimi
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

        // Fare tekerleğiyle silah geçişi
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            isMeleeWeaponSelected = !isMeleeWeaponSelected;
            SelectWeapon();
        }
    }

    /// <summary>
    /// Melee ve ranged silahların aktiflik durumunu günceller.
    /// </summary>
    void SelectWeapon()
    {
        if (meleeWeapon != null)
            meleeWeapon.SetActive(isMeleeWeaponSelected);

        if (rangedWeapon != null)
            rangedWeapon.SetActive(!isMeleeWeaponSelected);
    }

    /// <summary>
    /// Yeni bir ranged silah seçildiğinde envantere ekler, eskisini yok eder.
    /// </summary>
    /// <param name="newWeaponPrefab">Instantiate edilecek yeni silah prefab'ı</param>
    /// <param name="newType">Silahın enum tipi</param>
    /// <param name="position">Silahın yerel pozisyonu</param>
    /// <param name="rotation">Silahın yerel rotasyonu (Euler açılar)</param>
    public void SetNewRangedWeapon(GameObject newWeaponPrefab, WeaponPickUp.WeaponType newType, Vector3 position, Vector3 rotation)
    {
        // Önceki ranged silahı yok et
        if (rangedWeapon != null)
            Destroy(rangedWeapon);

        // Yeni silahı oluşturup WeaponHolder altında konumlandır
        rangedWeapon = Instantiate(newWeaponPrefab, weaponHolder);
        rangedWeapon.transform.localPosition = position;
        rangedWeapon.transform.localRotation = Quaternion.Euler(rotation);

        // Enum değerini güncelle
        currentRangedWeaponType = newType;

        // Şu anki seçim melee değilse (ranged seçiliyse) yeni silahı aktif et
        rangedWeapon.SetActive(!isMeleeWeaponSelected);
    }

    /// <summary>
    /// Envanterde tutulan ranged silahın enum değerini döner.
    /// </summary>
    public WeaponPickUp.WeaponType GetCurrentRangedWeaponType()
    {
        return currentRangedWeaponType;
    }
}
