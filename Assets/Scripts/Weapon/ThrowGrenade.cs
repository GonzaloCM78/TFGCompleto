using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{
    public GameObject grenadePrefab;
    public Transform spawnPoint;

    private float throwForce = 1000f;
    private int granadasRestantes = 4;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && granadasRestantes > 0 && Time.timeScale != 0)
        {
            granadasRestantes--;
            Throw();
        }
    }

    public void Throw()
    {
        GameObject newGrenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        newGrenade.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
    }
}
