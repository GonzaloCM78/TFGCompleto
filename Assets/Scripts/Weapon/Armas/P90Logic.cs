using System.Collections;
using UnityEngine;
using TMPro;

public class P90Logic : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject bullet;
    public float shootForce = 10000f;
    public float shootRate = 0.15f;
    public AudioClip shotSound;
    public AudioClip sonidoRecarga;

    private bool canShoot = true;
    private bool isReloading = false;

    private AudioSource audioSource;
    private Animator animator;

    public int damage = 8;

    public TextMeshProUGUI ammoText; // Asigna desde la escena

    public GameObject muzzleFlashPrefab;
    public Transform muzzleFlashPoint;

    // AK-47: 30 balas por cargador, 90 de reserva
    public AmmoData ammo = new AmmoData(50, 300);

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

        yield return new WaitForSeconds(3f); // Animación de recarga

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

        Bala balascript = newBullet.GetComponent<Bala>();

        if (balascript != null)
        {
            balascript.damage = damage;
        }

        Destroy(newBullet, 3f);

        // Instanciar la partícula de disparo (fogonazo)
        if (muzzleFlashPrefab != null && muzzleFlashPoint != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, muzzleFlashPoint.position, muzzleFlashPoint.rotation);
            Destroy(flash, 1f); // destruir tras 1 segundo
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
