using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private TextMeshProUGUI nameTextComponent; 
    [SerializeField] private Image portraitImageComponent;      
    
    [Header("Settings")]
    [SerializeField] private float textSpeed;
    
    private DialogueManager _dialogueManager;
    private ScriptableDialogue _currentDialogue;
    private CanvasGroup _canvasGroup; // We use this to hide/show visuals
    private int _index;

    void Awake()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();
        
        // Grab the CanvasGroup component
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            // If you forgot to add it, this will add it for you!
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Hide visuals immediately without disabling the script
        HideUI();
    }

    void OnEnable() { DialogueManager.OnDialogueStarted += LoadAndPlayDialogue; }
    void OnDisable() { DialogueManager.OnDialogueStarted -= LoadAndPlayDialogue; }

    private void LoadAndPlayDialogue(ScriptableDialogue newDialogue)
    {
        _currentDialogue = newDialogue;
        _index = 0;
        
        if (nameTextComponent != null) nameTextComponent.text = _currentDialogue.SpeakerName;

        if (portraitImageComponent != null)
        {
            portraitImageComponent.sprite = _currentDialogue.Portrait;
            portraitImageComponent.gameObject.SetActive(_currentDialogue.Portrait != null);
        }

        textComponent.text = string.Empty;
        
        ShowUI(); // Make the UI visible
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        // Only run if the UI is actually visible (alpha > 0)
        if (DialogueManager.DialogueActive && _canvasGroup.alpha > 0 && Input.GetMouseButtonDown(0))
        {
            string[] lines = _currentDialogue.GetDialogueLines();
            
            if (textComponent.text == lines[_index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[_index];
            }
        }
    }

    IEnumerator TypeLine()
    {
        string[] lines = _currentDialogue.GetDialogueLines();
        foreach (char c in lines[_index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine() 
    {
        string[] lines = _currentDialogue.GetDialogueLines();
        if (_index < lines.Length - 1)
        {
            _index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            HideUI();
            _dialogueManager.EndDialogue(); 
        }
    }

    private void ShowUI()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    private void HideUI()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}