using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAceite : MonoBehaviour
{
    [Header("Habilidades a Desbloquear")]
    [SerializeField] private bool unlockDoubleJump = true;
    [SerializeField] private bool unlockDash = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                if (unlockDoubleJump)
                {
                    player.UnlockDoubleJump();
                }
                if (unlockDash)
                {
                    player.UnlockDash();
                }
            }

            // Destruye el objeto coleccionable tras ser tomado
            Destroy(gameObject);
        }
    }
}
