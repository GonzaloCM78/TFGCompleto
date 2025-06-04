using UnityEngine;

public class DisableColliderOnOpen : MonoBehaviour
{
    public Animator animator; // El animator que controla la puerta
    public string openParam = "isOpened"; // Nombre del parámetro
    public float disableDelay = 1f; // Espera a que se mueva un poco antes de quitar el collider

    private Collider col;
    private bool colliderDisabled = false;

    void Start()
    {
        col = GetComponent<Collider>();
        if (animator == null)
        {
            Debug.LogWarning("Animator no asignado en DisableColliderOnOpen.");
        }
    }

    void Update()
    {
        if (!colliderDisabled && animator != null && animator.GetBool(openParam))
        {
            colliderDisabled = true;
            Invoke(nameof(DisableCollider), disableDelay);
        }
    }

    void DisableCollider()
    {
        if (col != null)
        {
            col.enabled = false;
        }
    }
}
