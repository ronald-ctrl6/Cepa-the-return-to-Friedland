using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [Header("Componentes de Animación")]
    [SerializeField] private Animator faderAnimator;
    [SerializeField] private float transitionTime = 1f;

    [Header("Nombres de Escenas")]
    [SerializeField] private string levelSceneName = "Level1-1"; // Cambia esto por el nombre exacto de tu primer nivel
    [SerializeField] private string menuSceneName = "Menu";     // Nombre de tu escena de menú

    void Start()
    {
        DestruirObjetoPorTag("Player");
        DestruirObjetoPorTag("UI");
    }

    private void DestruirObjetoPorTag(string tagName)
    {
        GameObject objetoPersistente = GameObject.FindWithTag(tagName);
        if (objetoPersistente != null)
        {
            Destroy(objetoPersistente);
        }
    }

    // Método para el Botón "Continuar"
    public void BotonContinuar()
    {
        StartCoroutine(LoadSceneWithFade(levelSceneName));
    }

    // Método para el Botón "Volver al Menú"
    public void BotonMenu()
    {
        StartCoroutine(LoadSceneWithFade(menuSceneName));
    }

    // Corrutina encargada de hacer el fundido (fade) antes de cambiar de escena
    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        if (faderAnimator != null)
        {
            faderAnimator.SetTrigger("StartFadeOut");
        }

        // Esperamos el tiempo de la animación
        yield return new WaitForSeconds(transitionTime);

        // Cargamos la escena correspondiente
        SceneManager.LoadScene(sceneName);
    }
}
