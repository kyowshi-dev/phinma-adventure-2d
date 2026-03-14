using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float _currentHealth;

    [SerializeField]
    private float _maximumHealth;

    public float RemainingHealthPercentage
    {
        get
        {
            return _currentHealth / _maximumHealth;
        }
    }

    public bool IsInvincible { get; set; }

    public UnityEvent OnDied;

    public UnityEvent OnDamaged;

    public UnityEvent OnHealthChanged;
    public void TakeDamage(float damageAmount)
    {
        if (_currentHealth == 0)
        {
            return;
        }

        if (IsInvincible)
        {
            return;
        }

        _currentHealth -= damageAmount;

        GetComponent<EnemyDamagedFlash>()?.StartFlash();

        OnHealthChanged.Invoke();

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        if (_currentHealth == 0)
        {
            OnDied.Invoke();
        }
        else
        {
            OnDamaged.Invoke();
        }
    }

    public void AddHealth(float amountToAdd)
    {
        if (_currentHealth == _maximumHealth)
        {
            return;
        }

        _currentHealth += amountToAdd;

        OnHealthChanged.Invoke();

        if (_currentHealth > _maximumHealth)
        {
            _currentHealth = _maximumHealth;
        }
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
}
