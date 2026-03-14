using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Scriptable Dialogue")]
public class ScriptableDialogue : ScriptableObject
{
    // Unique identifier for this dialogue
    [SerializeField]
    private string dialogueID;

    // Name of the character speaking
    [SerializeField]
    private string speakerName;

    // Optional portrait sprite for the character
    [SerializeField]
    private Sprite portrait;

    // Optional voice audio clip to accompany the dialogue
    [SerializeField]
    private AudioClip voiceClip;

    // List of dialogue lines to be displayed sequentially
    [SerializeField]
    [TextArea(3, 5)]
    private List<string> dialogueLines = new List<string>();

    // Public property to access dialogue ID
    public string DialogueID => dialogueID;

    // Public property to access speaker name
    public string SpeakerName => speakerName;

    // Public property to access portrait sprite
    public Sprite Portrait => portrait;

    // Public property to access voice audio clip
    public AudioClip VoiceClip => voiceClip;

    // Method to retrieve all dialogue lines as a string array (compatible with Dialogue.cs)
    public string[] GetDialogueLines()
    {
        return dialogueLines.ToArray();
    }

    // Method to retrieve all dialogue lines as a list
    public List<string> GetDialogueLinesList()
    {
        return new List<string>(dialogueLines);
    }

    // Method to get a specific dialogue line by index
    public string GetDialogueLine(int index)
    {
        if (index >= 0 && index < dialogueLines.Count)
        {
            return dialogueLines[index];
        }
        Debug.LogWarning($"Dialogue line index {index} is out of range for dialogue ID: {dialogueID}");
        return string.Empty;
    }

    // Method to get the total number of dialogue lines
    public int GetLineCount()
    {
        return dialogueLines.Count;
    }
}

// Example usage for Dialogue.cs:
/*
public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private ScriptableDialogue scriptableDialogue;
    [SerializeField]
    public TextMeshProUGUI textComponent;
    [SerializeField]
    public float textSpeed;

    private int index;

    void Start()
    {
        if (scriptableDialogue != null)
        {
            textComponent.text = string.Empty;
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        index = 0;
        string[] lines = scriptableDialogue.GetDialogueLines();
        StartCoroutine(TypeLine(lines));
    }

    IEnumerator TypeLine(string[] lines)
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        string[] lines = scriptableDialogue.GetDialogueLines();
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine(lines));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
*/
