using UnityEngine;
using UnityEngine.UI;

public class PowerUpAceite : MonoBehaviour
{
    [Header("Habilidades a Desbloquear")]
    [SerializeField] private bool unlockDoubleJump = true;
    [SerializeField] private bool unlockDash = true;

    [Header("Configuración de Inventario y Mensaje")]
    [SerializeField] private int itemIndex = 1;
    [SerializeField, TextArea] private string notificationMessage = "¡Botella de aceite conseguida! \n- Tienes doble salto \n- Impulsate con Shift";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Desbloquear habilidades
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                if (unlockDoubleJump) player.UnlockDoubleJump();
                if (unlockDash) player.UnlockDash();
            }

            // 2. Actualizar únicamente el slot del aceite en el UIManager
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateItem(itemIndex, UIManager.Instance.aceiteSprite);
                UIManager.Instance.ShowNotification(notificationMessage);
            }

            // 3. Destruir la botella del mapa
            Destroy(gameObject);
        }
    }
}
