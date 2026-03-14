using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Connections")]
    [Tooltip("Drag the GameObject holding your DialogueManager here.")]
    [SerializeField] private DialogueManager _dialogueManager;
    
    // The specific dialogue file you assign in the Inspector
    [SerializeField] private ScriptableDialogue _dialogueToPlay;

    private bool _hasBeenTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the trigger is the Player
        if (collision.CompareTag("Player") && !_hasBeenTriggered)
        {
            _hasBeenTriggered = true; // This zone is now "used up"
            _dialogueManager.StartDialogue(_dialogueToPlay);
            Debug.Log($"Player entered zone: Playing dialogue {_dialogueToPlay.DialogueID}!");
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the Player is leaving the trigger
        if (collision.CompareTag("Player"))
        {
            if (_dialogueManager != null)
            {
                _dialogueManager.EndDialogue();
                Debug.Log("Player left zone: Dialogue Ended!");
            }
        }
    }
}

