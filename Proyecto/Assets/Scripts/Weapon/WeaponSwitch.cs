using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public List<GameObject> ownedWeapons = new List<GameObject>();
    public int selectedWeapon = 0;

    void Start()
    {
        // Solo activa la primera (ej. pistola inicial)
        if (ownedWeapons.Count > 0)
        {
            for (int i = 0; i < ownedWeapons.Count; i++)
                ownedWeapons[i].SetActive(i == selectedWeapon);
        }
    }

    void Update()
    {
        if (ownedWeapons.Count == 0) return;

        int previousWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            selectedWeapon = (selectedWeapon + 1) % ownedWeapons.Count;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            selectedWeapon = (selectedWeapon - 1 + ownedWeapons.Count) % ownedWeapons.Count;
        }

        if (previousWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    public GameObject GetActiveWeapon()
    {
        foreach (GameObject weapon in ownedWeapons)
        {
            if (weapon.activeSelf)
                return weapon;
        }
        return null;
    }

    void SelectWeapon()
    {
        for (int i = 0; i < ownedWeapons.Count; i++)
        {
            bool isSelected = (i == selectedWeapon);
            ownedWeapons[i].SetActive(isSelected);

            if (isSelected)
            {
                var pistol = ownedWeapons[i].GetComponent<PistolaLogic>();
                var ak = ownedWeapons[i].GetComponent<AK47Logic>();
                var uzi = ownedWeapons[i].GetComponent<UZILogic>();

                if (pistol != null) pistol.UpdateAmmoUI();
                if (ak != null) ak.UpdateAmmoUI();
                if (uzi != null) uzi.UpdateAmmoUI();
            }
        }
    }

    //  LLAMAR A ESTA FUNCIÓN DESDE WeaponBuyZone cuando se compra un arma
    public void AddWeapon(GameObject newWeapon)
    {
        if (ownedWeapons.Contains(newWeapon))
        {
            Debug.Log("Ya tienes esta arma.");
            return;
        }

        // Si ya tienes 2, reemplaza el arma activa
        if (ownedWeapons.Count >= 2)
        {
            GameObject weaponToReplace = ownedWeapons[selectedWeapon];
            weaponToReplace.SetActive(false);
            ownedWeapons[selectedWeapon] = newWeapon;
            newWeapon.SetActive(true); // Activa la nueva automáticamente
        }
        else
        {
            // Si tienes menos de 2, la añades al final y la activas
            ownedWeapons.Add(newWeapon);
            selectedWeapon = ownedWeapons.Count - 1;
            SelectWeapon();
        }
    }

    public void ReplaceWeapon(GameObject oldWeapon, GameObject newWeapon)
    {
        int index = ownedWeapons.IndexOf(oldWeapon);
        if (index != -1)
        {
            ownedWeapons[index] = newWeapon;
        }
    }

    public void SelectWeaponByReference(GameObject target)
    {
        for (int i = 0; i < ownedWeapons.Count; i++)
        {
            bool active = (ownedWeapons[i] == target);
            ownedWeapons[i].SetActive(active);
            if (active)
            {
                selectedWeapon = i;
                var pistol = ownedWeapons[i].GetComponent<PistolaLogic>();
                var ak = ownedWeapons[i].GetComponent<AK47Logic>();
                var uzi = ownedWeapons[i].GetComponent<UZILogic>();
                if (pistol != null) pistol.UpdateAmmoUI();
                if (ak != null) ak.UpdateAmmoUI();
                if (uzi != null) uzi.UpdateAmmoUI();
            }
        }
    }


}
