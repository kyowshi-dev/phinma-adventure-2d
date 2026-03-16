using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectiveHighlight : MonoBehaviour
{
    [SerializeField]
    private Color _highlightColor = Color.yellow;

    [SerializeField]
    private float _glowSpeed = 2f;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Save the original color the moment this script gets turned on
        if (_spriteRenderer != null)
        {
            _originalColor = _spriteRenderer.color;
        }
    }

    private void Update()
    {
        // Mathf.PingPong creates a smooth wave between 0 and 1 over time
        float lerp = Mathf.PingPong(Time.time * _glowSpeed, 1f);
        
        // Blend between the normal color and the yellow highlight
        _spriteRenderer.color = Color.Lerp(_originalColor, _highlightColor, lerp);
    }

    private void OnDisable()
    {
        // Safety feature: Whenever this script is turned off, instantly reset the color back to normal
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = _originalColor;
        }
    }
}