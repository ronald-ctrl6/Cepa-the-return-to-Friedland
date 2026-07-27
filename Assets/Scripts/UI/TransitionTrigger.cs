using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionTrigger : MonoBehaviour
{
    private bool yaTransiciono = false; // Evita que se dispare dos veces si el player toca el trigger repetidamente

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !yaTransiciono)
        {
            yaTransiciono = true;

            int siguienteEscena = SceneManager.GetActiveScene().buildIndex + 1;

            if (TransitionManager.Instance != null)
            {
                // Le pedimos al mánager que haga su magia con el slide
                TransitionManager.Instance.CargarSiguienteEscena(siguienteEscena);
            }
            else
            {
                // Por si acaso no existe el mánager en la escena, cambia de golpe para no romper el juego
                SceneManager.LoadScene(siguienteEscena);
            }
        }
    }
}
