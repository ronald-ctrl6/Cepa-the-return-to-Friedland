using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHover : MonoBehaviour
{
    [Header("Hover")]
    [SerializeField] private float amplitude = 0.08f;
    [SerializeField] private float frequency = 6f;

    private Vector3 startLocalPos;

    private void Start()
    {
        startLocalPos = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = startLocalPos +
            Vector3.up * Mathf.Sin(Time.time * frequency) * amplitude;
    }
}