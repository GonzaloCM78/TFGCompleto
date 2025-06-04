using UnityEngine;

public class StatueBreakable : MonoBehaviour
{
    public GameObject breakEffect;
    public AudioClip breakSound;

    private bool isBroken = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isBroken) return;

        if (collision.gameObject.CompareTag("Bala"))
        {
            isBroken = true;

            if (breakSound != null)
                AudioSource.PlayClipAtPoint(breakSound, transform.position);

            if (breakEffect != null)
                Instantiate(breakEffect, transform.position, Quaternion.identity);

            FindObjectOfType<EasterEggManager>().OnStatueDestroyed();
            Destroy(gameObject);
        }
    }
}
