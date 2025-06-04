using UnityEngine;

public class WeaponBuyZone : MonoBehaviour
{
    public Transform player;
    public string weaponName = "AK47"; // Nombre base
    public int cost = 1000;
    public int ammoCost = 500;
    public float interactionDistance = 3f;

    private static WeaponBuyZone currentZone;

    public AudioSource audioSource;
    public AudioClip chiclin;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionDistance)
        {
            if (currentZone == null || currentZone == this)
            {
                currentZone = this;

                var weaponSwitch = Camera.main.GetComponent<WeaponSwitch>();
                bool alreadyOwned = false;

                foreach (var weapon in weaponSwitch.ownedWeapons)
                {
                    if (weapon.name == weaponName || weapon.name == weaponName + "_PAP")
                    {
                        alreadyOwned = true;
                        break;
                    }
                }

                string message = alreadyOwned
                    ? $"Pulsa F para comprar munición de {weaponName} - Coste: {ammoCost}"
                    : $"Pulsa F para comprar {weaponName} - Coste: {cost}";

                GameManager.Instance.ShowInteractionMessage(message);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    audioSource.PlayOneShot(chiclin);
                    TryBuyWeapon(alreadyOwned);
                }
            }
        }
        else if (currentZone == this)
        {
            currentZone = null;
            GameManager.Instance.ClearInteractionMessage();
        }
    }

    void TryBuyWeapon(bool alreadyOwned)
    {
        int finalCost = alreadyOwned ? ammoCost : cost;

        if (!GameManager.Instance.SpendPoints(finalCost)) return;

        GameManager.Instance.ClearInteractionMessage();

        var weaponSwitch = Camera.main.GetComponent<WeaponSwitch>();
        if (weaponSwitch == null) return;

        GameObject normalWeapon = FindWeaponInHierarchy(weaponName);
        GameObject papWeapon = FindWeaponInHierarchy(weaponName + "_PAP");

        if (!alreadyOwned)
        {
            if (normalWeapon != null)
            {
                normalWeapon.SetActive(true);
                weaponSwitch.AddWeapon(normalWeapon);
                Debug.Log($"{weaponName} comprada y activada.");
            }
            else
            {
                Debug.LogWarning($"No se encontró el arma '{weaponName}' en la jerarquía.");
            }

            return;
        }

        // Ya tiene el arma (normal o PAP)
        GameObject weaponToReload = papWeapon != null && papWeapon.activeSelf ? papWeapon : normalWeapon;
        bool isPAP = weaponToReload != null && weaponToReload.name.EndsWith("_PAP");

        if (weaponToReload == null)
        {
            Debug.LogWarning($"No se encontró ninguna versión activa del arma '{weaponName}'.");
            return;
        }

        MonoBehaviour[] scripts = weaponToReload.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            var ammoField = script.GetType().GetField("ammo");
            if (ammoField != null)
            {
                AmmoData ammo = ammoField.GetValue(script) as AmmoData;
                if (ammo != null)
                {
                    // Recarga con valores mejorados o normales
                    switch (weaponName)
                    {
                        case "Pistola":
                            ammo.reserveAmmo = isPAP ? 120 : 72;
                            break;
                        case "AK74":
                            ammo.reserveAmmo = isPAP ? 450 : 270;
                            break;
                        case "Uzi":
                            ammo.reserveAmmo = isPAP ? 420 : 250;
                            break;
                        case "M4":
                            ammo.reserveAmmo = isPAP ? 500 : 300;
                            break;
                        case "P90":
                            ammo.reserveAmmo = isPAP ? 500 : 300;
                            break;
                        
                    }

                    var updateMethod = script.GetType().GetMethod("UpdateAmmoUI");
                    if (updateMethod != null)
                        updateMethod.Invoke(script, null);

                    Debug.Log($"Munición {(isPAP ? "mejorada" : "normal")} recargada para {weaponToReload.name}");
                    break; // ¡Muy importante! Salimos del bucle una vez encontrado
                }
            }
        }
    }

    GameObject FindWeaponInHierarchy(string name)
    {
        Transform[] allChildren = player.GetComponentsInChildren<Transform>(true); // busca inactivos
        foreach (Transform child in allChildren)
        {
            if (child.name == name)
                return child.gameObject;
        }
        return null;
    }
}
