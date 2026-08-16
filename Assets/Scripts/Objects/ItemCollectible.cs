using System.Collections.Generic;
using UnityEngine;

public class ItemCollectible : MonoBehaviour
{
    public enum CollectibleType { Coin, Diamond, Character, Item }

    [Header("Configuración del Coleccionable")]
    [SerializeField] private CollectibleType type;
    [SerializeField] private string collectibleID; // ID único para cada coleccionable en todo el juego
    [SerializeField] private int amountValue = 1;  // Cuánto suma (para monedas o diamantes)

    [Header("Animación (Solo para Personajes)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string collectedAnimTrigger = "IsCollected"; // El nombre del Trigger en tu Animator

    // Lista estática global para recordar qué coleccionables ya fueron recogidos en toda la sesión
    private static HashSet<string> collectedItems = new HashSet<string>();

    void Start()
    {
        // Si este objeto ya fue recogido en otra escena o antes, aplicamos su estado guardado
        if (!string.IsNullOrEmpty(collectibleID) && collectedItems.Contains(collectibleID))
        {
            ApplyCollectedState();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!string.IsNullOrEmpty(collectibleID) && collectedItems.Contains(collectibleID)) return;

        // 1. Marcar como recogido en la lista global
        if (!string.IsNullOrEmpty(collectibleID))
        {
            collectedItems.Add(collectibleID);
        }

        // 2. Ejecutar la lógica según el tipo
        HandleCollection();
    }

    private void HandleCollection()
    {
        switch (type)
        {
            case CollectibleType.Coin:
                // Llama a tu GameManager o sistema de monedas
                // GameManager.Instance.AddCoins(amountValue);
                Destroy(gameObject);
                break;

            case CollectibleType.Diamond:
                // Llama a tu GameManager o sistema de diamantes
                // GameManager.Instance.AddGems(amountValue);
                Destroy(gameObject);
                break;

            case CollectibleType.Character:
                // Cambia la animación del personaje rescatado y no lo destruyas
                ApplyCollectedState();
                // GameManager.Instance.AddRescue(); // Para tu texto de rescates ("1 / 5")
                break;
            case CollectibleType.Item:
                // Llama a tu GameManager o sistema de diamantes
                // GameManager.Instance.AddGems(amountValue);
                Destroy(gameObject);
                break;
        }
    }

    private void ApplyCollectedState()
    {
        if (type == CollectibleType.Character)
        {
            if (animator != null)
            {
                animator.SetBool(collectedAnimTrigger, true); // O usa animator.Play("NombreDeAnimacion");
            }
            // Desactivamos el collider para que el jugador no pueda volver a interactuar con él
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }
        else
        {
            // Si es moneda o diamante y por alguna razón recarga la escena, se destruye
            Destroy(gameObject);
        }
    }
}