using System.Collections;
using UnityEngine;
using TMPro;

public class EscopetaLogic : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject bullet;
    public float shootForce = 10000f;
    public float shootRate = 1f;
    public AudioClip shotSound;

    private bool canShoot = true;
    private bool isReloading = false;

    private AudioSource audioSource;
    private Animator animator;

    public int pelletsPerShot = 8;
    public float spreadAngle = 6f;
    public int damagePerPellet = 2;

    public TextMeshProUGUI ammoText;

    public GameObject muzzleFlashPrefab;
    public Transform muzzleFlashPoint;

    public AmmoData ammo = new AmmoData(6, 60);

    private Coroutine reloadCoroutine;

    public Renderer shellRenderer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        if (shellRenderer != null)
            shellRenderer.enabled = false;

        UpdateAmmoUI();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && ammo.CanShoot() && Time.timeScale != 0)
        {
            if (isReloading)
            {
                StopReload(); // nueva función limpia y clara
            }

            if(gameObject.name == "M1014_PAP")
            {
                shootRate = 0.5f;
            }

            audioSource.PlayOneShot(shotSound);
            ammo.ConsumeAmmo();
            Shoot();
            animator.SetTrigger("fire");
            StartCoroutine(DelayDisparo());
            UpdateAmmoUI();
        }

        if (Input.GetKeyDown(KeyCode.R) && ammo.currentAmmo < ammo.magazineSize && ammo.reserveAmmo > 0 && !isReloading)
        {
            reloadCoroutine = StartCoroutine(ReloadBullets());
        }
    }

    IEnumerator DelayDisparo()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootRate);
        canShoot = true;
    }

    IEnumerator ReloadBullets()
    {
        isReloading = true;
        canShoot = false;
        animator.SetBool("isReloading", true);

        while (ammo.currentAmmo < ammo.magazineSize && ammo.reserveAmmo > 0)
        {
            yield return new WaitForSeconds(0.15f);

            

            ammo.currentAmmo++;
            ammo.reserveAmmo--;
            UpdateAmmoUI();

            if (shellRenderer != null)
                shellRenderer.enabled = true;

            yield return new WaitForSeconds(0.4f);

            if (shellRenderer != null)
                shellRenderer.enabled = false;
        }

        animator.SetBool("isReloading", false);
        isReloading = false;
        canShoot = true;
    }

    private void StopReload()
    {
        if (reloadCoroutine != null)
            StopCoroutine(reloadCoroutine);

        isReloading = false;
        canShoot = true;

        if (shellRenderer != null)
            shellRenderer.enabled = false;

        animator.SetBool("isReloading", false);
    }

    private void Shoot()
    {
        for (int i = 0; i < pelletsPerShot; i++)
        {
            Quaternion spreadRotation = Quaternion.Euler(
                Random.Range(-spreadAngle, spreadAngle),
                Random.Range(-spreadAngle, spreadAngle),
                0
            );

            Vector3 direction = spreadRotation * spawnPoint.forward;
            Quaternion finalRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f);

            GameObject pellet = Instantiate(bullet, spawnPoint.position, finalRotation);
            pellet.GetComponent<Rigidbody>().AddForce(direction * shootForce);

            Bala balascript = pellet.GetComponent<Bala>();
            if (balascript != null)
                balascript.damage = damagePerPellet;

            Destroy(pellet, 3f);
        }

        if (muzzleFlashPrefab != null && muzzleFlashPoint != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, muzzleFlashPoint.position, muzzleFlashPoint.rotation);
            Destroy(flash, 1f);
        }
    }

    public void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = $"{ammo.currentAmmo} / {ammo.reserveAmmo}";
    }

    private void OnDisable()
    {
        StopReload();

        animator.Rebind();
        animator.Update(0f);
    }
}
