using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;    
    }

    private void Update()
    {
        DestroyWhenOffScreen();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Check if the thing we hit is tagged as an Enemy
        if (collision.CompareTag("Enemy"))
        {
            // 2. Try to get the HealthController
            HealthController healthController = collision.GetComponent<HealthController>();
            
            // 3. If it has one, deal damage!
            if (healthController != null)
            {
                healthController.TakeDamage(10);
            }
            
            // 4. Destroy the bullet so it doesn't bounce or pierce
            DestroyBullet();
        }

        // Destroy on hitting walls
        if (collision.CompareTag("Wall"))
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void DestroyWhenOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if (screenPosition.x < 0 ||
            screenPosition.x > _camera.pixelWidth ||
            screenPosition.y < 0 ||
            screenPosition.y > _camera.pixelHeight)
        {
            Destroy(gameObject);
        }
    }
}