using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtSFX : MonoBehaviour
{
    [SerializeField] private AudioClip hurtSound;
    private AudioSource audioSource;
    private HealthController healthController;

    // Start is called before the first frame update
    void Start()
    {
        healthController = GetComponent<HealthController>();
        if (healthController == null)
        {
            Debug.LogError("HealthController not found on this GameObject!", gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on this GameObject!", gameObject);
        }

        if (healthController != null)
        {
            healthController.OnDamaged.AddListener(PlayHurtSound);
        }
    }

    private void PlayHurtSound()
    {
        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
