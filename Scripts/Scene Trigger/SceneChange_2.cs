using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange_2 : MonoBehaviour
{
    private SceneController _sceneController;

    private void Awake()
    {
        // Find the SceneController in the scene (assuming it's on a GameObject like a manager)
        _sceneController = FindObjectOfType<SceneController>();
        if (_sceneController == null)
        {
            Debug.LogError("SceneController not found in the scene!", gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Load scene 2 via the SceneController
            if (_sceneController != null)
            {
                _sceneController.LoadScene("Scene 3");
            }
        }
    }
}