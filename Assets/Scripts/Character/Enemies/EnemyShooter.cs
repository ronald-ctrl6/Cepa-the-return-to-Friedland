using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 2f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        Debug.Log("Timer: " + timer);

        if (timer >= fireRate)
        {
            Debug.Log("Entró al disparo");
            Shoot();
            timer = 0f;
        }
    }

    private void Shoot()
    {
        Debug.Log("Disparando");

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity);

        Debug.Log("Proyectil creado: " + projectile.name);

        EnemyProjectile bullet = projectile.GetComponent<EnemyProjectile>();

        if (bullet != null)
        {
            Debug.Log("EnemyProjectile encontrado");

            float dir = transform.localScale.x > 0 ? 1 : -1;
            bullet.SetDirection(Vector2.right * dir);
        }
        else
        {
            Debug.Log("NO tiene EnemyProjectile");
        }
    }
}