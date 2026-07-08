using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Botón Jugar
    public void Jugar()
    {
        SceneManager.LoadScene("Level1");
    }

    // Botón Continuar
    public void Continuar()
    {
        SceneManager.LoadScene("Level1");
    }

    // Botón Niveles
    public void Niveles()
    {
        Debug.Log("Abrir menú de niveles");
    }

    // Botón Opciones
    public void Opciones()
    {
        Debug.Log("Abrir opciones");
    }

    // Botón Créditos
    public void Creditos()
    {
        Debug.Log("Abrir créditos");
    }

    // Botón Salir
    public void Salir()
    {
        Application.Quit();

        // Para probar dentro del Editor
        Debug.Log("Salir del juego");
    }
}
