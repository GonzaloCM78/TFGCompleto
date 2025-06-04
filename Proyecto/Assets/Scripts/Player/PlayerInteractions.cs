using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private bool regenerandoVida = false;
    public GameObject maxAmmoPrefab; // arrástralo en el Inspector

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Vector3 spawnPos = transform.position + transform.forward * 4f + Vector3.up * 1f;
            Instantiate(maxAmmoPrefab, spawnPos, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GunAmmo"))
        {
           
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.LoseHealth(50);

            if (!regenerandoVida && GameManager.Instance.health < 100)
            {
                
                StartCoroutine(RegenerarVida());
            }
        }

       
    }

    IEnumerator RegenerarVida()
    {
        regenerandoVida = true;

        while (GameManager.Instance.health < 100)
        {
            yield return new WaitForSeconds(5f);
            GameManager.Instance.RegenHealth(50);
            Debug.Log("Vida regenerada");
        }

        regenerandoVida = false;

        
    }
}
