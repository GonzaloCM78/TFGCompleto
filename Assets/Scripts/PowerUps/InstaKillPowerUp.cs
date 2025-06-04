using UnityEngine;

public class InstaKillPowerUp : MonoBehaviour
{
    public float duration = 20f;
    public AudioSource audioSource;
    public AudioClip loopSound;
    public AudioClip cogerPowerUp;
    public AudioClip instaKill;
    

    void Start()
    {
        if (audioSource != null && loopSound != null)
        {
            audioSource.clip = loopSound;
            audioSource.loop = true;
            audioSource.Play(); // Comienza a sonar al aparecer
        }

        Destroy(gameObject, 30f); // Desaparece si no se recoge
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Detener sonido en bucle
        if (audioSource != null)
        {
            audioSource.Stop();

            if (cogerPowerUp != null)
            {
                audioSource.PlayOneShot(cogerPowerUp);
                audioSource.PlayOneShot(instaKill);
            }
        }

        //  Ocultar visualmente el objeto
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
            meshRenderer.enabled = false;

        // También ocultar hijos si contienen parte visual
        foreach (Transform child in transform)
        {
            MeshRenderer childRenderer = child.GetComponent<MeshRenderer>();
            if (childRenderer != null)
                childRenderer.enabled = false;
        }

        // Desactivar colisión
        GetComponent<Collider>().enabled = false;

        GameManager.Instance.ActivateInstaKill(duration);
        GameManager.Instance.ShowTemporaryMessage("¡Baja instantánea!", 2f);

        // Destruir tras reproducir sonido (espera 1.5s)
        Destroy(gameObject, 3f);
    }
}
