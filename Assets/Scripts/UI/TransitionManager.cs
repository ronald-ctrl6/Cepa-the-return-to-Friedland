using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{

    public static TransitionManager Instance { get; private set; }

    [Header("Referencias de Transición")]
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private float transitionTime = 0.5f; // Ajusta según la duración de tu animación StartTransition

    private bool isTransitioning = false; // Evita que se dispare la transición más de una vez a la vez

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Cargar escena por Índice (ej: 0, 1, 2)
    public void CargarSiguienteEscena(int indexEscena)
    {
        if (isTransitioning) return; // Si ya está cambiando de escena, ignora llamadas extra
        StartCoroutine(RutinaEscena(indexEscena));
    }

    // Cargar escena por Nombre (ej: "Nivel1", "MenuPrincipal")
    public void CargarSiguienteEscena(string nombreEscena)
    {
        if (isTransitioning) return;
        StartCoroutine(RutinaEscenaNombre(nombreEscena));
    }

    private IEnumerator RutinaEscena(int indexEscena)
    {
        isTransitioning = true;

        // 1. Iniciar animación de salida (cubrir la pantalla)
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("StartTransition");
        }

        // 2. Esperar a que la animación de cubrir pantalla termine
        yield return new WaitForSeconds(transitionTime);

        // 3. Cargar la escena de forma asíncrona en segundo plano
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(indexEscena);

        // Esperamos a que la escena se cargue por completo
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 4. Iniciar animación de entrada (destapar la pantalla)
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("EndTransition");
        }

        isTransitioning = false;
    }

    private IEnumerator RutinaEscenaNombre(string nombreEscena)
    {
        isTransitioning = true;

        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("StartTransition");
        }

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nombreEscena);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("EndTransition");
        }

        isTransitioning = false;
    }
}
