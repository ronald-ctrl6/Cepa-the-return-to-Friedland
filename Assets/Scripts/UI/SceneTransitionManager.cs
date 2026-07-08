using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Componentes de Animación")]
    [SerializeField] private Animator faderAnimator;
    [SerializeField] private float transitionTime = 1f;

    [Header("Nombre de la Escena del Primer Nivel")]
    [SerializeField] private string nextSceneName;

    // Este método se asignará al botón "COMENZAR"
    public void PlayGame()
    {
        StartCoroutine(LoadSceneRoutine(nextSceneName));
    }

    // Corrutina para sincronizar la animación con la carga física de la escena
    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // 1. Activamos el trigger del Animator para que la pantalla se vuelva negra
        if (faderAnimator != null)
        {
            faderAnimator.SetTrigger("StartFadeOut");
        }

        // 2. Esperamos el tiempo exacto que dura la animación de Fade Out
        yield return new WaitForSeconds(transitionTime);

        // 3. Cargamos de forma segura la nueva escena del juego
        SceneManager.LoadScene(sceneName);
    }

    // Este método se asignará al botón "SALIR"
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit(); // Nota: Esto solo funciona en la build compilada, no dentro del editor
    }
}
