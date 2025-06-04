using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packa : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip sonidoPaca;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            audioSource.clip = sonidoPaca;
            audioSource.Play();

        }
    }
}
