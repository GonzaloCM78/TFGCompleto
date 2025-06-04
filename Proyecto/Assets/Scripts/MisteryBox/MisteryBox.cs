using System.Collections;
using UnityEngine;

public class MisteryBox : MonoBehaviour
{
    public Animator controller;
    public Animation movement;
    public GameObject[] guns; // Armas visuales
    public Transform cubePosition;
    public Transform player;

    [Header("Interacción")]
    public Transform interactionPoint;
    public float interactionDistance = 3f;

    private int selectedGun = -1;
    private float animationTimer = 0f;
    private float timer = 0f;
    private bool isReadyToPickup = false;
    private bool boxInUse = false;
    private bool playerInZone = false;

    public float boxUseCooldown = 3f;
    private bool canUseBox = true;

    public int boxCost = 950;

    void Start()
    {
        controller = GetComponentsInChildren<Animator>()[0];
        movement = GetComponentsInChildren<Animation>()[0];
    }

    void Update()
    {
        float distance = Vector3.Distance(interactionPoint.position, player.position);

        if (distance <= interactionDistance && !boxInUse && canUseBox)
        {
            if (!playerInZone)
            {
                GameManager.Instance.ShowInteractionMessage($"Pulsa F para usar la caja - Coste: {boxCost}");
                playerInZone = true;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                TryUseBox();
            }
        }
        else if (playerInZone && (distance > interactionDistance || boxInUse))
        {
            GameManager.Instance.ClearInteractionMessage();
            playerInZone = false;
        }

        if (isReadyToPickup)
        {
            float pickupDistance = Vector3.Distance(interactionPoint.position, player.position);
            if (pickupDistance <= interactionDistance)
            {
                if (!playerInZone)
                {
                    GameManager.Instance.ShowInteractionMessage("Pulsa F para recoger el arma");
                    playerInZone = true;
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    TryBuyRandomWeapon();
                }
            }
            else if (playerInZone)
            {
                GameManager.Instance.ClearInteractionMessage();
                playerInZone = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (boxInUse && movement.IsPlaying("gunMovement"))
        {
            animationTimer += Time.deltaTime;
            timer += Time.deltaTime;

            if (timer >= 0.2f)
            {
                RandomizeWeapon();
                timer = 0f;
            }

            if (IsValidGunIndex(selectedGun))
                guns[selectedGun].transform.position = cubePosition.position;
        }
        else if (boxInUse && !isReadyToPickup && animationTimer >= 9.5f)
        {
            isReadyToPickup = true;
            Debug.Log("Caja lista para recoger arma");

            if (IsValidGunIndex(selectedGun))
                guns[selectedGun].transform.position = cubePosition.position;
        }
    }

    void TryUseBox()
    {
        if (!GameManager.Instance.SpendPoints(boxCost)) return;

        GameManager.Instance.ClearInteractionMessage();

        boxInUse = true;
        canUseBox = false;
        animationTimer = 0f;
        timer = 0f;
        selectedGun = -1;
        isReadyToPickup = false;

        OpenLid();
        movement.Play();
        RandomizeWeapon(); // Activamos al menos un arma visual ya desde el inicio

        StartCoroutine(CloseLidAfterDelay(9f));
        StartCoroutine(ResetBoxCooldown(boxUseCooldown));
    }

    void TryBuyRandomWeapon()
    {
        if (!IsValidGunIndex(selectedGun)) return;

        string visualName = guns[selectedGun].name;
        string realWeaponName = visualName.Replace("_Visual", "");

        GameObject weaponObj = FindWeaponInHierarchy(realWeaponName);
        if (weaponObj == null)
        {
            Debug.LogWarning($"No se encontró el arma '{realWeaponName}' como hija de la cámara.");
            return;
        }

        var weaponSwitch = Camera.main.GetComponent<WeaponSwitch>();
        bool alreadyOwned = weaponSwitch.ownedWeapons.Exists(w => w.name == realWeaponName);

        if (!alreadyOwned)
        {
            weaponSwitch.AddWeapon(weaponObj);
            weaponObj.SetActive(true);
            Debug.Log($"Arma '{realWeaponName}' recogida y activada.");
        }
        else
        {
            Debug.Log($"Ya tenías el arma '{realWeaponName}'. No se ha duplicado.");
        }

        isReadyToPickup = false;
        GameManager.Instance.ClearInteractionMessage();
        CloseLid();
        DisableGuns();
    }

    IEnumerator CloseLidAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isReadyToPickup && IsValidGunIndex(selectedGun))
        {
            guns[selectedGun].SetActive(false);
            isReadyToPickup = false;
        }

        CloseLid();
        boxInUse = false;
    }

    IEnumerator ResetBoxCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canUseBox = true;
    }

    void RandomizeWeapon()
    {
        if (guns.Length == 0) return;

        int rand = Random.Range(0, guns.Length);
        while (rand == selectedGun && guns.Length > 1)
        {
            rand = Random.Range(0, guns.Length);
        }

        selectedGun = rand;
        DisableGuns();

        if (IsValidGunIndex(selectedGun))
        {
            guns[selectedGun].SetActive(true);
            guns[selectedGun].transform.position = cubePosition.position;
        }
    }

    void DisableGuns()
    {
        foreach (GameObject gun in guns)
            gun.SetActive(false);
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

    void OpenLid()
    {
        controller.Play("OpenLid");
    }

    void CloseLid()
    {
        controller.Play("CloseLid");
    }

    bool IsValidGunIndex(int index)
    {
        return index >= 0 && index < guns.Length;
    }
}
