using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private float _screenBorder;

    [SerializeField]
    private float _obstacleCheckCircleRadius;

    [SerializeField]
    private float _obstacleCheckDistance;

    [SerializeField]
    private LayerMask _obstacleLayerMask;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;
    private float _changeDirectionCooldown;
    private Camera _camera;
    private RaycastHit2D[] _obstacleCollisions;
    private float _obstacleAvoidanceCooldown;
    private Vector2 _obstacleAvoidanceTargetDirection;
    private Vector2 _lastValidDirection;
    private float _stuckTimer;
    private float _stuckThreshold = 0.5f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up;
        _camera = Camera.main;
        _obstacleCollisions = new RaycastHit2D[10];
        
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody2D component not found on enemy!", gameObject);
            enabled = false;
        }
        
        if (_playerAwarenessController == null)
        {
            Debug.LogError("PlayerAwarenessController component not found on enemy!", gameObject);
        }
        
        if (_camera == null)
        {
            Debug.LogError("Main camera not found in scene!", gameObject);
        }
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        HandlePlayerTargeting();
        HandleObstacles();
        HandleEnemyOffScreen();
    }

    private void HandleRandomDirectionChange()
    {
        _changeDirectionCooldown -= Time.deltaTime;

        if (_changeDirectionCooldown <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;

            _changeDirectionCooldown = Random.Range(1f, 5f);
        }
    }

    private void HandlePlayerTargeting()
    {
        if (_playerAwarenessController != null && _playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
    }

    private void HandleEnemyOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < _screenBorder && _targetDirection.x < 0) ||
            (screenPosition.x > _camera.pixelWidth - _screenBorder && _targetDirection.x > 0))
        {
            _targetDirection = new Vector2(-_targetDirection.x, _targetDirection.y);
        }

        if ((screenPosition.y < _screenBorder && _targetDirection.y < 0) ||
            (screenPosition.y > _camera.pixelHeight - _screenBorder && _targetDirection.y > 0))
        {
            _targetDirection = new Vector2(_targetDirection.x, -_targetDirection.y);
        }
    }

    private void HandleObstacles()
    {
        _obstacleAvoidanceCooldown -= Time.deltaTime;
        _stuckTimer += Time.deltaTime;

        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_obstacleLayerMask);

        int numberOfCollisions = Physics2D.CircleCast(
            transform.position,
            _obstacleCheckCircleRadius,
            _targetDirection,
            contactFilter,
            _obstacleCollisions,
            _obstacleCheckDistance);

        if (numberOfCollisions > 0)
        {
            var obstacleCollision = _obstacleCollisions[0];

            if (obstacleCollision.collider.gameObject == gameObject)
            {
                return;
            }

            // If stuck for too long, try alternative directions
            if (_stuckTimer > _stuckThreshold)
            {
                Vector2 bestDirection = FindBestAvoidanceDirection(obstacleCollision.normal);
                _obstacleAvoidanceTargetDirection = bestDirection;
                _stuckTimer = 0;
                _obstacleAvoidanceCooldown = 0.3f;
            }
            else if (_obstacleAvoidanceCooldown <= 0)
            {
                _obstacleAvoidanceTargetDirection = obstacleCollision.normal;
                _obstacleAvoidanceCooldown = 0.3f;
            }

            var targetRotation = Quaternion.LookRotation(transform.forward, _obstacleAvoidanceTargetDirection);
            var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            _targetDirection = rotation * Vector2.up;
        }
        else
        {
            _stuckTimer = 0;
        }
    }

    private Vector2 FindBestAvoidanceDirection(Vector2 obstacleNormal)
    {
        // Try multiple directions to find the best escape route
        Vector2[] avoidanceDirections = new Vector2[]
        {
            obstacleNormal,
            Rotate(obstacleNormal, 45f),
            Rotate(obstacleNormal, -45f),
            Rotate(obstacleNormal, 90f),
            Rotate(obstacleNormal, -90f)
        };

        Vector2 bestDirection = obstacleNormal;
        int minCollisions = int.MaxValue;

        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_obstacleLayerMask);

        foreach (Vector2 direction in avoidanceDirections)
        {
            int collisions = Physics2D.CircleCast(
                transform.position,
                _obstacleCheckCircleRadius,
                direction,
                contactFilter,
                _obstacleCollisions,
                _obstacleCheckDistance);

            if (collisions < minCollisions)
            {
                minCollisions = collisions;
                bestDirection = direction;
            }

            // If we found a clear path, use it immediately
            if (collisions == 0)
            {
                break;
            }
        }

        return bestDirection;
    }

    private Vector2 Rotate(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(vector.x * cos - vector.y * sin, vector.x * sin + vector.y * cos);
    }

    private void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _rigidbody.SetRotation(rotation);
    }

    private void SetVelocity()
    {
        _rigidbody.velocity = transform.up * _speed;
    }
}
