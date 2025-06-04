using UnityEngine;

public class NukePowerUp : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip loopSound;
    public AudioClip cogerPowerUp;
    public AudioClip kaboom;

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
                audioSource.PlayOneShot(kaboom);
            }
        }

        AI[] allZombies = FindObjectsOfType<AI>();

        foreach (AI zombie in allZombies)
        {
            if (zombie != null)
                zombie.ForceDie(); // Necesita existir en AI.cs
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

        GameManager.Instance.ShowTemporaryMessage("¡KABOOM!", 2f);

        Destroy(gameObject, 3f);
    }
}
