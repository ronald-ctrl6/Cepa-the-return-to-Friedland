using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamara : MonoBehaviour
{
    [Header("Objetivo a Seguir")]
    [SerializeField] private Transform target; // Arrastra aquí a tu Personaje

    [Header("Configuración de Seguimiento")]
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Configuración Auto-Move (Opcional)")]
    [SerializeField] private bool autoMoveX;
    [SerializeField] private bool autoMoveY;
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;

    private float autoOffsetX = 0f;
    private float autoOffsetY = 0f;

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Si el auto-move está activo, calculamos un desplazamiento acumulativo en el tiempo
        if (autoMoveX) autoOffsetX += speedX * Time.deltaTime;
        if (autoMoveY) autoOffsetY += speedY * Time.deltaTime;

        // 2. La posición ideal se basa en la posición REAL del personaje + el offset base + el desplazamiento automático
        float targetX = autoMoveX ? target.position.x + autoOffsetX : target.position.x;
        float targetY = autoMoveY ? target.position.y + autoOffsetY : target.position.y;

        Vector3 desiredPosition = new Vector3(targetX, targetY, 0f) + offset;

        // 3. Interpolación suave para evitar tirones de cámara
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 4. Aplicamos la posición final
        transform.position = smoothedPosition;
    }
}
