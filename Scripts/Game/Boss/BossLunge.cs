using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLunge : MonoBehaviour
{
    [SerializeField]
    private float _lungeSpeed = 15f;

    [SerializeField]
    private float _lungeDuration = 0.5f;

    [SerializeField]
    private float _lungeCooldown = 3f;

    [SerializeField]
    private float _lungeCheckDistance = 5f;

    [SerializeField]
    private float _lungeCheckRadius = 0.5f;

    [SerializeField]
    private LayerMask _solidLayerMask;

    private PlayerAwarenessController _playerAwarenessController;
    private Rigidbody2D _rigidbody;
    private bool _isLunging = false;
    private float _lungeTimer;
    private float _cooldownTimer;
    private Vector2 _lungeDirection;
    private RaycastHit2D[] _lungeCollisions;

    private void Awake()
    {
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _lungeCollisions = new RaycastHit2D[10];

        if (_playerAwarenessController == null)
        {
            Debug.LogError("PlayerAwarenessController not found on boss!", gameObject);
            enabled = false;
        }

        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody2D not found on boss!", gameObject);
            enabled = false;
        }
    }

    private void Update()
    {
        if (_isLunging)
        {
            _lungeTimer -= Time.deltaTime;
            if (_lungeTimer <= 0)
            {
                StopLunge();
            }
        }
        else
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0 && CanLunge())
            {
                StartLunge();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isLunging)
        {
            _rigidbody.velocity = _lungeDirection * _lungeSpeed;
        }
    }

    private bool CanLunge()
    {
        if (_playerAwarenessController == null || !_playerAwarenessController.AwareOfPlayer)
            return false;

        Vector2 direction = _playerAwarenessController.DirectionToPlayer;
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_solidLayerMask);

        int numberOfCollisions = Physics2D.CircleCast(
            transform.position,
            _lungeCheckRadius,
            direction,
            contactFilter,
            _lungeCollisions,
            _lungeCheckDistance);

        return numberOfCollisions == 0;
    }

    private void StartLunge()
    {
        _isLunging = true;
        _lungeTimer = _lungeDuration;
        _lungeDirection = _playerAwarenessController.DirectionToPlayer;
        _cooldownTimer = _lungeCooldown;
    }

    private void StopLunge()
    {
        _isLunging = false;
        _rigidbody.velocity = Vector2.zero;
    }
}

