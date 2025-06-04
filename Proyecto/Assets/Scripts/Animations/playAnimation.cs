using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAnimation : MonoBehaviour
{

    public bool play;
    ParticleSystem[] dust;

    void Start()
    {
        dust = GetComponentsInChildren<ParticleSystem>();
    }

  
    void Update()
    {
        if (play)
        {
            dust[0].Play();
            dust[1].Play();

            play = false;
        }
    }
}
