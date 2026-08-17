using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private const int SCENA_GANAR = 6;
    private const int SCENA_PERDER = 7;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Suscribirse correctamente al evento
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // La firma correcta es: (Scene scene, LoadSceneMode mode)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Comparamos el índice de la escena cargada
        if (scene.buildIndex == SCENA_GANAR || scene.buildIndex == SCENA_PERDER)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Buena práctica: quitar el evento si el objeto se destruye
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
