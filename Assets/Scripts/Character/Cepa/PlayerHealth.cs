using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    public int maxHearts = 3;
    private int currentHearts;

    [Header("Respawn")]
    public Transform respawnPoint; // El punto donde reaparecerá

    [Header("Referencias UI")]
    public UIManager uiManager;

    private bool isTakingDamage = false;

    void Start()
    {
        currentHearts = maxHearts;
        if (uiManager == null) uiManager = FindFirstObjectByType<UIManager>();

        UpdateUI();
    }

    // Método para restar vida y hacer respawn
    public void TakeDamage(int damageAmount)
    {
        if (isTakingDamage) return; // Si ya está procesando daño, ignora el siguiente

        isTakingDamage = true;
        currentHearts -= damageAmount;

        UpdateUI();
        if (currentHearts <= 0)
        {
            // Si la salud llega a 0, cargamos la escena de GameOver
            SceneManager.LoadScene("Game Over"); // Asegúrate de que el nombre sea exacto
        }
        else
        {
            Respawn();
            Invoke("ResetDamageState", 0.5f);
        }
    }

    void ResetDamageState() { isTakingDamage = false; }

    public void Respawn()
    {
        if (respawnPoint != null)
        {
            // Desactiva el Rigidbody temporalmente para evitar bugs de física durante el teletransporte
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero; // En Unity 6 usa linearVelocity, en versiones anteriores es velocity
            }

            transform.position = respawnPoint.position;
        }
        else
        {
            Debug.LogWarning("No se ha asignado un Respawn Point.");
        }
    }

    void UpdateUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateHealth(currentHearts);
        }
    }

    // Método opcional por si necesitas consultar la vida desde otro script
    public int GetCurrentHearts()
    {
        return currentHearts;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Busca el objeto llamado "RespawnPoint" en la nueva escena
        GameObject spawnPointObj = GameObject.Find("RespawnPoint");
        if (spawnPointObj != null)
        {
            respawnPoint = spawnPointObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró ningún objeto llamado 'RespawnPoint' en esta escena.");
        }
    }
}
