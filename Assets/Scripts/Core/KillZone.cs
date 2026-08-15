using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Solo aplicar daño si es el jugador
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            // Añadimos una validación para evitar que se ejecute si ya se está procesando
            if (playerHealth != null)
            {
                // Usamos un pequeño booleano o simplemente nos aseguramos que 
                // el PlayerHealth no esté haciendo respawn en este preciso microsegundo
                playerHealth.TakeDamage(1);
            }
        }
    }
}
