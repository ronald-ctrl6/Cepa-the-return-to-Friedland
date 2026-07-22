using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Progreso del Jugador")]
    public bool hasDoubleJump = false;
    public bool hasDash = false;

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
        }
    }
}
