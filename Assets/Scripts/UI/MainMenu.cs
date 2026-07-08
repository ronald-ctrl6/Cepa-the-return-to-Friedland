using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Componentes de Animación")]
    [SerializeField] private Animator faderAnimator;
    [SerializeField] private float transitionTime = 1f;

    [Header("Nombre de la Escena del Primer Nivel")]
    [SerializeField] private string nextSceneName = "Level1";


    // Botón Jugar
    public void Play()
    {
        Debug.Log("EL BOTÓN PLAY FUE PRESIONADO FÍSICAMENTE");
        StartCoroutine(LoadSceneWithFade(nextSceneName));

        // En lugar de cargar directo, iniciamos la secuencia con Fade
        StartCoroutine(LoadSceneWithFade(nextSceneName));
    }

    // Botón Continuar
    public void Continuar()
    {
        StartCoroutine(LoadSceneWithFade(nextSceneName));
    }

    // Corrutina encargada de esperar la animación antes del cambio físico
    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        if (faderAnimator != null)
        {
            // Activamos el trigger para pasar de FadeIn (transparente) a FadeOut (negro)
            faderAnimator.SetTrigger("StartFadeOut");
        }

        // Esperamos el tiempo exacto que configuraste en el inspector (ej. 1 segundo)
        yield return new WaitForSeconds(transitionTime);

        // Una vez la pantalla está en negro, cargamos la escena de forma segura
        SceneManager.LoadScene(sceneName);
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
