using UnityEngine;
using System.Collections;

public class PackAPunchZone : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 3f;
    public int upgradeCost = 5000;

    public Animator animator;
    public float animationDuration = 4f;

    private static PackAPunchZone currentZone;
    private bool isProcessing = false;

    private GameObject pendingUpgradeWeapon;
    private GameObject upgradedWeapon;

    public AudioSource audioSource;
    public AudioClip sonidoPaca;

    void Update()
    {
        if (player == null || isProcessing) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionDistance)
        {
            if (currentZone == null || currentZone == this)
            {
                currentZone = this;

                var weaponSwitch = Camera.main.GetComponent<WeaponSwitch>();
                GameObject currentWeapon = weaponSwitch.GetActiveWeapon();

                if (currentWeapon == null)
                {
                    GameManager.Instance.ShowInteractionMessage("No tienes un arma equipada.");
                    return;
                }

                if (currentWeapon.name.EndsWith("_PAP"))
                {
                    GameManager.Instance.ShowInteractionMessage("Este arma ya está mejorada.");
                    return;
                }

                string upgradedName = currentWeapon.name + "_PAP";

                GameManager.Instance.ShowInteractionMessage($"Pulsa F para mejorar {currentWeapon.name} - Coste: {upgradeCost}");

                if (Input.GetKeyDown(KeyCode.F))
                {
                    audioSource.clip = sonidoPaca;
                    audioSource.Play();
                    TryUpgradeWeapon(currentWeapon, upgradedName, weaponSwitch);
                }
            }
        }
        else if (currentZone == this)
        {
            currentZone = null;
            GameManager.Instance.ClearInteractionMessage();
        }
    }

    void TryUpgradeWeapon(GameObject currentWeapon, string upgradedName, WeaponSwitch weaponSwitch)
    {
        if (!GameManager.Instance.SpendPoints(upgradeCost)) return;

        upgradedWeapon = FindWeaponInHierarchy(upgradedName);
        if (upgradedWeapon == null)
        {
            Debug.LogWarning($"No se encontró el arma mejorada: {upgradedName}");
            return;
        }

        // 1. Quitar arma actual de la lista y desactivarla
        weaponSwitch.ownedWeapons.Remove(currentWeapon);
        currentWeapon.SetActive(false);
        pendingUpgradeWeapon = currentWeapon;

        isProcessing = true;
        GameManager.Instance.ClearInteractionMessage();

        // 2. Iniciar animación
        if (animator != null)
        {
            Debug.Log("Animator encontrado, se activa 'openBox'");
            animator.SetTrigger("openBox");
        }
        else
        {
            Debug.LogWarning("Animator es NULL en PackAPunchZone");
        }


        StartCoroutine(FinishUpgradeAfterDelay(weaponSwitch));
    }

    IEnumerator FinishUpgradeAfterDelay(WeaponSwitch weaponSwitch)
    {
        yield return new WaitForSeconds(animationDuration);

        // 3. Añadir arma mejorada a la lista y activarla
        if (!weaponSwitch.ownedWeapons.Contains(upgradedWeapon))
            weaponSwitch.ownedWeapons.Add(upgradedWeapon);

        weaponSwitch.SelectWeaponByReference(upgradedWeapon);

        if (animator != null)
        {
            animator.SetBool("isOpen", false);
            animator.SetTrigger("closeBox");
        }

        isProcessing = false;
    }

    GameObject FindWeaponInHierarchy(string name)
    {
        Transform[] allChildren = player.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            if (child.name == name)
                return child.gameObject;
        }
        return null;
    }
}
