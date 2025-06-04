using System.Collections;
using UnityEngine;

public class MaxAmmoPowerUp : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip loopSound;
    public AudioClip cogerPowerUp;
    public AudioClip maxAmmo;

    void Start()
    {
        if (audioSource != null && loopSound != null)
        {
            audioSource.clip = loopSound;
            audioSource.loop = true;
            audioSource.Play(); // Comienza a sonar al aparecer
        }

        Destroy(gameObject, 30f); // Desaparece tras 10 segundos si no se recoge
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Detener sonido en bucle
        if (audioSource != null)
        {
            audioSource.Stop();

            if (cogerPowerUp != null)
            {
                audioSource.PlayOneShot(cogerPowerUp);
                audioSource.PlayOneShot(maxAmmo);
            }
        }


        WeaponSwitch weaponSwitch = Camera.main.GetComponent<WeaponSwitch>();
        if (weaponSwitch == null) return;

        foreach (GameObject weapon in weaponSwitch.ownedWeapons)
        {
            var weaponScripts = weapon.GetComponents<MonoBehaviour>();

            foreach (var script in weaponScripts)
            {
                var ammoField = script.GetType().GetField("ammo");
                if (ammoField != null)
                {
                    AmmoData ammo = ammoField.GetValue(script) as AmmoData;
                    if (ammo != null)
                    {
                        ammo.RefillReserve();
                        if (ammo.currentAmmo == 0)
                            ammo.Reload(full: true);

                        var updateUI = script.GetType().GetMethod("UpdateAmmoUI");
                        if (updateUI != null && weapon.activeSelf)
                            updateUI.Invoke(script, null);
                    }
                }
            }
        }

        // Ocultar TODOS los MeshRenderers (incluso anidados)
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(true);
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        // Desactivar TODOS los colliders (incluyendo hijos)
        Collider[] colliders = GetComponentsInChildren<Collider>(true);
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Desactivar colisión
        GetComponent<Collider>().enabled = false;


        GameManager.Instance.ShowTemporaryMessage("¡Munición Máxima!", 2f);
        Destroy(gameObject, 3f);
    }

    
}
