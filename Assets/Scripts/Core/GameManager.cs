using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Progreso del Jugador")]
    public bool hasDoubleJump = false;
    public bool hasDash = false;

    [Header("Contadores de Coleccionables")]
    [SerializeField] private int totalCoins = 0;
    [SerializeField] private int totalGems = 0;
    [SerializeField] private int currentRescues = 0;
    [SerializeField] private int totalRescuesInLevel = 6;

    [Header("Referencias")]
    private UIManager uiManager;

    private void Awake()
    {
        // Patrón Singleton para que solo exista un GameManager y no se duplique al cambiar de escena
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Hace que este objeto sobreviva entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        // Nos suscribimos al evento de cambio de escena para buscar automáticamente el UIManager nuevo
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Se ejecuta cada vez que se carga una nueva escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Buscamos el UIManager de la escena actual
        uiManager = FindFirstObjectByType<UIManager>();

        // Refrescamos los textos de la UI inmediatamente al entrar a la escena
        UpdateAllUI();
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        if (uiManager != null) uiManager.UpdateCoins(totalCoins);
    }

    public void AddGems(int amount)
    {
        totalGems += amount;
        if (uiManager != null) uiManager.UpdateGems(totalGems);
    }

    public void AddRescue()
    {
        currentRescues++;
        if (uiManager != null) uiManager.UpdateRescues(currentRescues, totalRescuesInLevel);
    }

    private void UpdateAllUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateCoins(totalCoins);
            uiManager.UpdateGems(totalGems);
            uiManager.UpdateRescues(currentRescues, totalRescuesInLevel);
        }
    }
}
