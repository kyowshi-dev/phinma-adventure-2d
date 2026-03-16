using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private SceneController _sceneController;

    public void PlayNewGame()
    {
        // Wipe everything for a truly fresh start
        PlayerPrefs.DeleteKey("HasCheckpoint");
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("SavedScene"); // Don't forget to wipe the scene memory too!
        PlayerPrefs.Save();

        // Always load the very first level for a new game
        _sceneController.LoadScene("Game"); 
    }

    public void TryAgain()
    {
        // Ask the computer what scene we were in. 
        // If it can't find one (like if they die before hitting a checkpoint), default to "Game"
        string sceneToLoad = PlayerPrefs.GetString("SavedScene", "Game");

        // Load the saved level!
        _sceneController.LoadScene(sceneToLoad);
    }

    public void ReturnToMainMenu()
    {
        _sceneController.LoadScene("Main Menu");
    }

    public void Exit()
    {
        Application.Quit();
    }
}