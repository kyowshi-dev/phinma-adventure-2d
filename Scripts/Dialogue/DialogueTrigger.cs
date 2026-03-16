using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Connections")]
    [Tooltip("Drag the GameObject holding your DialogueManager here.")]
    [SerializeField] private DialogueManager _dialogueManager;
    
    // The specific dialogue file you assign in the Inspector
    [SerializeField] private ScriptableDialogue _dialogueToPlay;

    private bool _hasBeenTriggered = false;

    // Fired at the start of the dialogue so other systems can respond.
    public UnityEvent OnDialogueStarted;

    // Fired after the dialogue has been ended/closed so other systems (like cameras) can react.
    public UnityEvent OnDialogueEnded;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the trigger is the Player
        if (collision.CompareTag("Player") && !_hasBeenTriggered)
        {
            _hasBeenTriggered = true; // This zone is now "used up"
            _dialogueManager.StartDialogue(_dialogueToPlay);
            SaveCheckpoint();           
            OnDialogueStarted?.Invoke();
            Debug.Log($"Player entered zone: Playing dialogue {_dialogueToPlay.name}!");
        }
    }

    public void PlayDialogueOnDemand()
    {
        if (_dialogueManager != null && _dialogueToPlay != null)
        {
            _dialogueManager.StartDialogue(_dialogueToPlay);
            
            // ADD THIS HERE TOO: Save the game if a boss dies!
            SaveCheckpoint();
            
            OnDialogueStarted?.Invoke();
            Debug.Log($"Event triggered dialogue: {_dialogueToPlay.name}");
        }
        else
        {
            Debug.LogWarning("DialogueManager or DialogueToPlay is missing!");
        }
    }

    public void EndDialogue()
    {
        if (_dialogueManager != null)
        {
            _dialogueManager.EndDialogue();
            Debug.Log("Dialogue Ended!");
        }

        // Tell Unity (Inspector/other listeners) the dialogue sequence has ended.
        OnDialogueEnded?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the Player is leaving the trigger
        if (collision.CompareTag("Player"))
        {
            EndDialogue();
        }
    }

    // This saves the player's location to the computer's memory
    public void SaveCheckpoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerPrefs.SetFloat("CheckpointX", player.transform.position.x);
            PlayerPrefs.SetFloat("CheckpointY", player.transform.position.y);
            PlayerPrefs.SetInt("HasCheckpoint", 1);

            // ADD THIS LINE: Save the name of the current level
            PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name); 

            PlayerPrefs.Save();
            Debug.Log("Checkpoint and Scene Saved!");
        }
    }
}

