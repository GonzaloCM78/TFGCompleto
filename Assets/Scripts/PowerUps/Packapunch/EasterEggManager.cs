using UnityEngine;

public class EasterEggManager : MonoBehaviour
{
    public int totalStatues = 3;
    private int brokenStatues = 0;

    public GameObject packAPunchDoor1; // Asigna el objeto de la puerta en el Inspector
    public GameObject packAPunchDoor2; // Asigna el objeto de la puerta en el Inspector
    public AudioClip successSound;
    public AudioSource audioSource;

    public void OnStatueDestroyed()
    {
        brokenStatues++;

        if (brokenStatues >= totalStatues)
        {
            if (successSound != null && audioSource != null)
                audioSource.PlayOneShot(successSound);

            OpenPackAPunch();
        }
    }

    void OpenPackAPunch()
    {
        if (packAPunchDoor1 != null || packAPunchDoor2 != null)
            packAPunchDoor1.SetActive(false); // Desactiva la puerta visualmente
            packAPunchDoor2.SetActive(false); // Desactiva la puerta visualmente

        Debug.Log("¡Easter Egg completado! Pack-a-Punch desbloqueado.");
    }
}
