using System.Collections;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public GameObject bloodEffect; // Asigna en el Inspector
    public int damage = 1;         //  Este se asignar� desde el arma

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Instanciar part�cula en el punto de impacto
            ContactPoint contact = collision.contacts[0];
            GameObject effect = Instantiate(bloodEffect, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(effect, 2f);

            // Aplicar da�o configurable
            AI zombie = collision.gameObject.GetComponent<AI>();
            if (zombie != null)
            {
                zombie.LoseLife(damage);
            }

            Destroy(gameObject);
        }
    }
}
