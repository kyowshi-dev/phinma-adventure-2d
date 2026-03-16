using System.Collections;
using UnityEngine;

public class BossTeleportGimmick : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField]
    private float _vanishDuration = 0.5f;

    [SerializeField]
    private float _minTeleportDistance = 4f; // Prevents teleporting directly on top of the player

    [SerializeField]
    private float _maxTeleportDistance = 8f; // Keeps the boss from teleporting off-screen

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private Transform _playerTransform;
    private bool _isTeleporting = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        
        // Automatically find the player using the "Player" tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure your Player is tagged as 'Player'.");
        }
    }

    // We will trigger this method using your HealthController's OnDamaged event!
    public void TriggerTeleport()
    {
        if (!_isTeleporting && _playerTransform != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(TeleportSequence());
        }
    }

    private IEnumerator TeleportSequence()
    {
        _isTeleporting = true;

        // 1. Vanish (Turn off visuals and hitboxes so it can't take damage while invisible)
        if (_spriteRenderer != null) _spriteRenderer.enabled = false;
        if (_collider != null) _collider.enabled = false;

        // 2. Wait in the void
        yield return new WaitForSeconds(_vanishDuration);

        // 3. Calculate a new position safely away from the player
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(_minTeleportDistance, _maxTeleportDistance);
        
        // 4. Move the boss
        Vector3 newPosition = _playerTransform.position + (Vector3)(randomDirection * randomDistance);
        transform.position = newPosition;

        // 5. Reappear
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
        if (_collider != null) _collider.enabled = true;

        _isTeleporting = false;
    }
}