using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{

    // Instancia accesible desde cualquier script (Singleton sencillo)
    public static TransitionManager Instance { get; private set; }

    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private float transitionTime = 0.3f; // Tiempo que tarda en tapar la pantalla

    private void Awake()
    {
        // Nos aseguramos de que solo exista un Manager de transiciones entre escenas
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

    public void CargarSiguienteEscena(int indexEscena)
    {
        StartCoroutine(RutinaEscena(indexEscena));
    }
    private IEnumerator RutinaEscena(int indexEscena)
    {
        transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(indexEscena);
        transitionAnimator.SetTrigger("EndTransition");
    }
}
