using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    public Transform weaponHolder;
    public int maxWeapons = 2;

    private List<GameObject> ownedWeapons = new List<GameObject>();
    private int currentWeaponIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddWeapon(GameObject weapon)
    {
        string weaponName = weapon.name;

        // Verificamos si ya tiene esta arma
        GameObject existingWeapon = ownedWeapons.Find(w => w.name == weaponName);
        if (existingWeapon != null)
        {
            ReplenishAmmo(existingWeapon);
            return;
        }

        if (ownedWeapons.Count >= maxWeapons)
        {
            RemoveWeapon(currentWeaponIndex); // Reemplazar arma activa
        }

        GameObject newWeapon = Instantiate(weapon, weaponHolder);
        newWeapon.layer = LayerMask.NameToLayer("Weapon");
        newWeapon.SetActive(false); // Se activa por el WeaponSwitch

        ownedWeapons.Add(newWeapon);
        SwitchToWeapon(ownedWeapons.Count - 1);
    }

    private void ReplenishAmmo(GameObject weapon)
    {
        var pistol = weapon.GetComponent<PistolaLogic>();
        var ak = weapon.GetComponent<AK47Logic>();
        var uzi = weapon.GetComponent<UZILogic>();

        if (pistol != null) pistol.ammo.Reload(true);
        if (ak != null) ak.ammo.Reload(true);
        if (uzi != null) uzi.ammo.Reload(true);
    }

    private void RemoveWeapon(int index)
    {
        if (index < ownedWeapons.Count)
        {
            Destroy(ownedWeapons[index]);
            ownedWeapons.RemoveAt(index);
        }
    }

    public void SwitchToWeapon(int index)
    {
        if (index < 0 || index >= ownedWeapons.Count) return;

        for (int i = 0; i < ownedWeapons.Count; i++)
        {
            ownedWeapons[i].SetActive(i == index);
        }

        currentWeaponIndex = index;
    }

    public List<GameObject> GetOwnedWeapons()
    {
        return ownedWeapons;
    }

    public int GetCurrentWeaponIndex()
    {
        return currentWeaponIndex;
    }
}
