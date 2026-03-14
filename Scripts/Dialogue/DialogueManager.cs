using System;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static bool DialogueActive { get; private set; }
    
    // Event to freeze/unfreeze player and enemies
    public static event Action<bool> OnDialogueStateChanged;
    
    // NEW: Event to send the specific dialogue data to the UI
    public static event Action<ScriptableDialogue> OnDialogueStarted;

    public void StartDialogue(ScriptableDialogue dialogueData)
    {
        DialogueActive = true;
        OnDialogueStateChanged?.Invoke(true); // Freeze game
        OnDialogueStarted?.Invoke(dialogueData); // Send data to UI
    }

    public void EndDialogue()
    {
        DialogueActive = false;
        OnDialogueStateChanged?.Invoke(false); // Unfreeze game
    }
}