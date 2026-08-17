using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReverseTransitionTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int targetIndex = SceneManager.GetActiveScene().buildIndex - 1;

            // Si existe el TransitionManager y el índice es válido (no negativo)
            if (TransitionManager.Instance != null && targetIndex >= 0)
            {
                // Llamamos al método que activa la animación y luego carga la escena
                TransitionManager.Instance.CargarSiguienteEscena(targetIndex);
            }
            else
            {
                // Fallback por si acaso: si no hay manager, carga directa para que no se trabe el juego
                SceneManager.LoadScene(targetIndex);
            }
        }
    }
}
