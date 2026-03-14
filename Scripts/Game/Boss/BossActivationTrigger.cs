using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivationTrigger : MonoBehaviour
{
    [Header("Target Boss")]
    [SerializeField]
    [Tooltip("Drag the Boss GameObject here to reference its movement script.")]
    private BossMovement bossMovementScript;

    [Header("Trigger Area")]
    [SerializeField]
    [Tooltip("Drag the BoxCollider2D being used as the trigger area here.")]
    private BoxCollider2D activationTrigger;

    private void Start()
    {
        // Prevent the boss from moving as soon as the game starts
        if (bossMovementScript != null)
        {
            bossMovementScript.enabled = false;
        }
        else
        {
            Debug.LogWarning("BossMovement script is not assigned in the Inspector!", gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger has the "Player" tag
        if (collision.CompareTag("Player"))
        {
            // Wake up the boss!
            if (bossMovementScript != null)
            {
                bossMovementScript.enabled = true;
            }

            // Optional: Disable this trigger so it doesn't run this code again
            if (activationTrigger != null)
            {
                activationTrigger.enabled = false;
            }
        }
    }
}