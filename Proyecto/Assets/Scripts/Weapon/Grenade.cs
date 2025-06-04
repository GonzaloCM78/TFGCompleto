using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public float delay = 3;
    float countdown;
    public float radius = 5;
    public float exploscionForce = 70;
    bool exploded = false;
    public GameObject explosionEffect;

    private AudioSource audioSource;
    public AudioClip grenadeSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        countdown = delay;
    }

    
    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 0 && exploded == false)
        {
            Exploded();
            exploded = true;
        }


        
    }


    void Exploded()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var rangeObjects in colliders)
        {

            AI ai = rangeObjects.GetComponent<AI>();

            if(ai != null)
            {
                ai.GrenadeImpact();
            }

            Rigidbody rb = rangeObjects.GetComponent<Rigidbody>();

            if (rb != null)
            {
               
                rb.AddExplosionForce(exploscionForce * 100, transform.position, radius);
            }

        }

        audioSource.PlayOneShot(grenadeSound);

        gameObject.GetComponent<SphereCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        
        Destroy(gameObject, delay*2);

    }
}
