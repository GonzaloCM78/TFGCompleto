using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PistolaLogic : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject bullet;
    private float shootForce = 10000f;
    private float shootRate = 0.5f;
    public AudioClip shotSound;
    public AudioClip reloadSound;

    private bool canShoot = true;
    private bool isReloading = false;

    private AudioSource audioSource;
    private Animator animator;

    public AmmoData ammo = new AmmoData(8, 72); // Pistola: 8 en cargador, 72 en reserva
    public int damage = 3;

    public TextMeshProUGUI ammoText;

    public GameObject muzzleFlashPrefab;
    public Transform muzzleFlashPoint; // donde aparecerá la partícula

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
            audioSource.PlayOneShot(reloadSound);
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
            StartCoroutine(Reload());
            audioSource.PlayOneShot(reloadSound);
        }

        if(gameObject.name == "Pistola_PAP")
        {
            shootRate = 0.1f;
        }
        {

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

        yield return new WaitForSeconds(1.5f); // Duración de la animación de recarga
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

        Bala balaScript = newBullet.GetComponent<Bala>();
        if (balaScript != null)
        {
            balaScript.damage = damage;
        }

        Destroy(newBullet, 3f);

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
        if (isReloading)
        {
            StopAllCoroutines();
            animator.SetBool("isReloading", false);
            isReloading = false;
            canShoot = true;
        }

        animator.Rebind();
        animator.Update(0f);
    }
}
