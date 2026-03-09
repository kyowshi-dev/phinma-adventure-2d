using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; } 

    public Vector2 DirectionToPlayer { get; private set; }

    [SerializeField]
    private float _playerAwarenessDistance;

    private Transform _player;

    void Awake()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement not found in scene! PlayerAwarenessController cannot function.", gameObject);
            enabled = false;
            return;
        }
        
        _player = playerMovement.transform;
    }

    void Update()
    {
        if (_player == null)
            return;
        
        Vector2 enemyToPlayerVector = _player.position - transform.position;
        float distanceToPlayer = enemyToPlayerVector.magnitude;
        
        DirectionToPlayer = distanceToPlayer > 0 ? enemyToPlayerVector.normalized : Vector2.zero;
        AwareOfPlayer = distanceToPlayer <= _playerAwarenessDistance;
    }
}
