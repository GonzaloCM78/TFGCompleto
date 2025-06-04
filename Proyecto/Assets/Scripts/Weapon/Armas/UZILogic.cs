using System.Collections;
using UnityEngine;
using TMPro;

public class UZILogic : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject bullet;
    private float shootForce = 10000f;
    private float shootRate = 0.1f;
    public AudioClip shotSound;
    public AudioClip sonidoRecarga;

    private bool canShoot = true;
    private bool isReloading = false;

    private AudioSource audioSource;
    private Animator animator;

    public int damage = 8;

    public TextMeshProUGUI ammoText; // Asignar en Inspector

    public GameObject muzzleFlashPrefab;
    public Transform muzzleFlashPoint;

    // UZI: 25 balas por cargador, 200 en reserva
    public AmmoData ammo = new AmmoData(25, 200);

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        UpdateAmmoUI();
    }

    void Update()
    {
        if (isReloading) return;

        // Forzar recarga automática si se queda sin balas
        if (ammo.currentAmmo == 0 && ammo.reserveAmmo > 0)
        {
            StartCoroutine(Reload());
            audioSource.PlayOneShot(sonidoRecarga);
            return;
        }

        // Disparo
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && ammo.CanShoot() && Time.timeScale != 0)
        {
            audioSource.PlayOneShot(shotSound);
            ammo.ConsumeAmmo();
            Shoot();
            animator.SetTrigger("fire");
            StartCoroutine(DelayDisparo());
            UpdateAmmoUI();
        }

        // Recarga manual
        if (Input.GetKeyDown(KeyCode.R) && ammo.currentAmmo < ammo.magazineSize && ammo.reserveAmmo > 0)
        {
            audioSource.PlayOneShot(sonidoRecarga);
            StartCoroutine(Reload());
        }
    }

    IEnumerator DelayDisparo()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootRate);
        canShoot = true;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("isReloading", true);
        canShoot = false;

        yield return new WaitForSeconds(2f); // Duración de la animación de recarga

        ammo.Reload();

        animator.SetBool("isReloading", false);
        isReloading = false;
        canShoot = true;

        UpdateAmmoUI();
    }

    private void Shoot()
    {
        Quaternion nuevaRotacion = spawnPoint.rotation * Quaternion.Euler(90f, 0, 0);
        GameObject newBullet = Instantiate(bullet, spawnPoint.position, nuevaRotacion);
        newBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.rotation * Vector3.forward * shootForce);

        Bala balascript = bullet.GetComponent<Bala>();

        if (balascript != null)
        {
            balascript.damage = damage;
        }

        Destroy(newBullet, 3f);

        if(muzzleFlashPrefab != null && muzzleFlashPoint != null)
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
        // Cancelar recarga si está activa
        if (isReloading)
        {
            StopAllCoroutines(); // Asegura que ninguna rutina quede corriendo
            animator.SetBool("isReloading", false);
            isReloading = false;
            canShoot = true;
        }

        // Resetear el animator (muy importante para que vuelva a Idle)
        animator.Rebind();
        animator.Update(0f);
    }



}
