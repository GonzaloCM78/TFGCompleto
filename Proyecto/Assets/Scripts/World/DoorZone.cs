using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorZone : MonoBehaviour
{
    public int cost = 1000;
    public Transform player;
    public float interactionDistance = 3f;
  //  public Animator animator; // Asigna un animator si vas a animar la barrera
    public GameObject barrierToRemove;
    public GameObject[] spawnPointsToActivate;

    private static DoorZone currentZone;

    private bool isOpened = false;

    public Animator animator;

    public AudioSource audioSource;
    public AudioClip openDoor;





    void Update()
    {
        if (isOpened) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionDistance)
        {
            // Solo una barrera puede mostrar el mensaje
            if (currentZone == null || currentZone == this)
            {
                currentZone = this;
                GameManager.Instance.ShowInteractionMessage($"Pulsa F para limpiar la barrera - Coste: {cost} puntos");

                if (Input.GetKeyDown(KeyCode.F))
                {
                    audioSource.PlayOneShot(openDoor);
                    TryOpenDoor();
                }
            }
        }
        else
        {
            // Solo esta barrera puede limpiar su propio mensaje
            if (currentZone == this)
            {
                currentZone = null;
                GameManager.Instance.ClearInteractionMessage();
            }
        }
    }



    void TryOpenDoor()
    {
        if (GameManager.Instance.SpendPoints(cost))
        {
            isOpened = true;

            GameManager.Instance.ClearInteractionMessage();

            if (animator != null)
            {
                animator.SetBool("isOpened", true);
            }

            foreach (var sp in spawnPointsToActivate)
            {
                sp.SetActive(true);
            }

            StartCoroutine(DestroyBarrierDelayed(2f));
        }
    }


    IEnumerator DestroyBarrierDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (barrierToRemove != null)
        {
            Destroy(barrierToRemove);
        }
    }
}
