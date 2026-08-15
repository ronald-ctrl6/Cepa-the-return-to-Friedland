using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hEART : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerHealth playerHealth; // Arrastra aquí a tu personaje
    public List<Image> heartsList;    // Arrastra aquí las imágenes de los corazones desde la jerarquía

    void Start()
    {
        // Si no asignas el PlayerHealth en el inspector, intenta buscarlo automáticamente
        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }
    }

    void Update()
    {
        if (playerHealth != null)
        {
            UpdateHearts(playerHealth.GetCurrentHearts());
        }
    }

    void UpdateHearts(int currentHearts)
    {
        for (int i = 0; i < heartsList.Count; i++)
        {
            if (i < currentHearts)
            {
                heartsList[i].enabled = true;  // Muestra el corazón si aún tiene vida
            }
            else
            {
                heartsList[i].enabled = false; // Oculta el corazón si se ha perdido
            }
        }
    }
}
