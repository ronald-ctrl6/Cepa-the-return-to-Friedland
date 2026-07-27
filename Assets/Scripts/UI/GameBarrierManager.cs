using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameBarrierManager : MonoBehaviour
{
    public static GameBarrierManager Instance { get; private set; }

    [Header("Configuración de Transición")]
    public Animator curtainAnimator;
    public float transitionTime = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void IrAPantalla(string nombreEscena)
    {
        StartCoroutine(CurtainRoutine(nombreEscena));
    }

    private IEnumerator CurtainRoutine(string sceneName)
    {
        if (curtainAnimator != null)
        {
            curtainAnimator.SetTrigger("StartFadeOut");
        }

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && spawnPoint != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            Collider2D playerCollider = player.GetComponent<Collider2D>();

            // 1. DESACTIVAR COLISIONES TEMPORALMENTE para evitar que el mapa lo "escupe"
            if (playerCollider != null)
            {
                playerCollider.enabled = false;
            }

            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.isKinematic = true; // Volvemos el cuerpo cinemático para que ignore la gravedad y fuerzas en este frame
            }

            // 2. Teletransportar de forma segura al plano limpio
            player.transform.position = new Vector3(
                spawnPoint.transform.position.x,
                spawnPoint.transform.position.y,
                0f // Aseguramos plano Z neutro para colisiones 2D
            );

            // 3. Posicionar las cámaras en el mismo sitio exacto
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                mainCam.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, mainCam.transform.position.z);
            }

            CinemachineVirtualCamera vCam = FindObjectOfType<CinemachineVirtualCamera>();
            if (vCam != null)
            {
                vCam.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, vCam.transform.position.z);
                vCam.Follow = player.transform;
                vCam.LookAt = player.transform;
                vCam.OnTargetObjectWarped(player.transform, player.transform.position - vCam.transform.position);
                vCam.PreviousStateIsValid = false;
            }

            // 4. Esperar un frame corto de Unity a que todo se asiente en las nuevas coordenadas
            yield return new WaitForEndOfFrame();

            // 5. REACTIVAR FÍSICAS una vez posicionado de forma segura
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.velocity = Vector2.zero;
            }
            if (playerCollider != null)
            {
                playerCollider.enabled = true;
            }
        }

        if (curtainAnimator == null)
        {
            curtainAnimator = GameObject.Find("CurtainCanvas")?.GetComponent<Animator>();
        }

        if (curtainAnimator != null)
        {
            curtainAnimator.SetTrigger("StartFadeIn");
        }
    }
}