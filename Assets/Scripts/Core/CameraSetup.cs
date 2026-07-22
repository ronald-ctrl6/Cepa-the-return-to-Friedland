using Cinemachine;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    private CinemachineVirtualCamera vCam; 

    void Start()
    {
        // Se obtiene la Virtual Camera
        vCam = GetComponent<CinemachineVirtualCamera>();
        if (vCam == null)
        {
            Debug.LogError("No se encontró CinemachineVirtualCamera en este GameObject.", this);
            return;
        }

        // Se busca al objeto con el Tag "Player"
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            // Asignamos el transform
            vCam.Follow = player.transform;

            // Asegurar Look At vacío para evitar rotaciones en 2D
            vCam.LookAt = null;
        }
        else
        {
            Debug.LogWarning("CinemachineAutoFollow: No se encontró ningún GameObject con el Tag 'Player'.");
        }
    }
}
