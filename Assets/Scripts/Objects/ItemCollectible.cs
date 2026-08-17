using System.Collections.Generic;
using UnityEngine;

public class ItemCollectible : MonoBehaviour
{
    public enum CollectibleType { Coin, Diamond, Character }

    [Header("Configuración del Coleccionable")]
    [SerializeField] private CollectibleType type;
    [SerializeField] private string collectibleID; // ID único para cada coleccionable en todo el juego
    [SerializeField] private int amountValue = 1;  // Cuánto suma (para monedas o diamantes)

    [Header("Animación (Solo para Personajes)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string collectedAnimTrigger = "IsCollected"; // El nombre del Trigger en tu Animator

    void Start()
    {
        // Consultamos al GameManager si este objeto ya fue recogido en la partida actual
        if (GameManager.Instance != null && GameManager.Instance.IsCollected(collectibleID))
        {
            ApplyCollectedState();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.Instance != null && GameManager.Instance.IsCollected(collectibleID)) return;

        // 1. Marcar como recogido en el GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MarkAsCollected(collectibleID);
        }

        // 2. Ejecutar la lógica según el tipo
        HandleCollection();
    }

    private void HandleCollection()
    {
        switch (type)
        {
            case CollectibleType.Coin:
                GameManager.Instance.AddCoins(amountValue);
                Destroy(gameObject);
                break;

            case CollectibleType.Diamond:
                GameManager.Instance.AddGems(amountValue);
                Destroy(gameObject);
                break;

            case CollectibleType.Character:
                ApplyCollectedState();
                GameManager.Instance.AddRescue();
                break;
        }
    }

    private void ApplyCollectedState()
    {
        if (type == CollectibleType.Character)
        {
            if (animator != null)
            {
                animator.SetTrigger(collectedAnimTrigger);
            }
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}