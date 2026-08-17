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
    public Transform respawnPoint;

    [Header("Referencias UI")]
    public UIManager uiManager;

    private bool isTakingDamage = false;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.currentHealth <= 0)
            {
                GameManager.Instance.currentHealth = maxHearts;
            }
            currentHearts = GameManager.Instance.currentHealth;
        }
        else
        {
            currentHearts = maxHearts;
        }

        if (uiManager == null) uiManager = FindFirstObjectByType<UIManager>();

        UpdateUI();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isTakingDamage) return;

        isTakingDamage = true;
        currentHearts -= damageAmount;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentHealth = currentHearts;
        }

        UpdateUI();

        if (currentHearts <= 0)
        {
            SceneManager.LoadScene("Game Over");
        }
        else
        {
            Invoke("ResetDamageState", 0.5f);
        }
    }

    void ResetDamageState() { isTakingDamage = false; }

    public void FallIntoPit()
    {
        TakeDamage(1);
        Respawn();
    }

    public void Respawn()
    {
        if (respawnPoint != null)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
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
