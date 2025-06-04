using System.Collections;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject bullet;
    public float shootForce = 10000f;
    public float shootRate = 0.5f;

    protected bool canShoot = true;
    protected AudioSource audioSource;
    public AudioClip shotSound;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && Time.timeScale != 0)
        {
            audioSource.PlayOneShot(shotSound);
            Shoot();
            StartCoroutine(DelayShoot());
        }
    }

    protected IEnumerator DelayShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootRate);
        canShoot = true;
    }

    protected virtual void Shoot()
    {
        Quaternion nuevaRotacion = spawnPoint.rotation * Quaternion.Euler(90f, 0, 0);
        GameObject newBullet = Instantiate(bullet, spawnPoint.position, nuevaRotacion);
        newBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.rotation * Vector3.forward * shootForce);
        Destroy(newBullet, 3f);
    }

    public virtual void Reload()
    {
        Debug.Log("Recargando...");
    }
}
