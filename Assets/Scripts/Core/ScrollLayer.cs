using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScrollLayer : MonoBehaviour
{
    [SerializeField] private Transform cam; // Cámara a seguir
    [SerializeField] private float parallaxMultiplierX = 0.5f; // Velocidad del parallax
    [SerializeField] private float parallaxMultiplierY = 0.1f;
    [SerializeField] private float constantSpeed = 0f; // Velocidad constante (ej: -1.5 para ir a la izquierda)

    private float spriteWidth; // Ancho del fondo
    private Vector3 lastCamPos; // última posición de la cámara

    private Transform[] backgrounds = new Transform[2];

    private void Start()
    {
        // Se verifica que las capas Layer tengan como mínimo 2 fondos, sino tira error
        if (transform.childCount != 2)
        {
            Debug.LogError($"ParallaxScrollerLayer only works with 2 Bg children, Fix '{name}' GameObject");
            return;
        }

        // Obtiene los fondos
        for (int i = 0; i < 2; i++) {
            backgrounds[i] = transform.GetChild(i);
        }

        // Verificación para la cámara. Si no existe una camara, hagarra la principal.
        // Toma la última posicion de la cámara  (principal o no) y la toma para si
        if (cam == null) cam = Camera.main.transform;
        lastCamPos = cam.position;

        // Obtiene el acnho del sprite del background automáticamente
        spriteWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        // Mueve el fondo de la cámara
        Vector3 deltaMovement = cam.position - lastCamPos;

        float movementX = (deltaMovement.x * parallaxMultiplierX) + (constantSpeed * Time.deltaTime);
        float movementY = deltaMovement.y * parallaxMultiplierY;

        transform.position += new Vector3(movementX, movementY, 0);
        lastCamPos = cam.position;

        //Reposiciona los fondos si la cámara avanza demasiado
        foreach (var bg in backgrounds){
            float camDistance = cam.position.x - bg.position.x;
            if (Mathf.Abs(camDistance) >= spriteWidth) {
                float offset = (camDistance > 0) ? spriteWidth * 2f : -spriteWidth * 2f;
                bg.position += new Vector3(offset, 0, 0);
            }
        }
    }

}
