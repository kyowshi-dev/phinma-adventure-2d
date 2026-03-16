using UnityEngine;
using UnityEngine.SceneManagement; // We need this to check the current level's name!

public class CheckpointLoader : MonoBehaviour
{
    private void Start()
    {
        // 1. Check if a checkpoint exists at all
        if (PlayerPrefs.GetInt("HasCheckpoint", 0) == 1)
        {
            // 2. Ask the computer what scene the checkpoint belongs to
            string savedScene = PlayerPrefs.GetString("SavedScene", "");
            string currentScene = SceneManager.GetActiveScene().name;

            // 3. ONLY teleport if the current level matches the saved level
            if (savedScene == currentScene)
            {
                float savedX = PlayerPrefs.GetFloat("CheckpointX");
                float savedY = PlayerPrefs.GetFloat("CheckpointY");
                
                transform.position = new Vector3(savedX, savedY, transform.position.z);
                Debug.Log($"Loaded checkpoint in {currentScene} at {savedX}, {savedY}");
            }
            else
            {
                // We are in a new level! Ignore the old checkpoint and stay where the Editor put us.
                Debug.Log($"Entered new scene ({currentScene}). Ignoring old checkpoint from {savedScene}.");
            }
        }
    }
}