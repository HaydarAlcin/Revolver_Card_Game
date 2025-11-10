using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Revolver Card Game")]
    [SerializeField] private AudioSource revolverAudioSource;
    [SerializeField] private AudioClip revolverTriggerClip;

    private void OnEnable()
        => EventManager.CoreAudioSignals.OnPlayRevolverTriggerSFX += OnPlayRevolverTriggerSFX;

    private void OnDisable()
        => EventManager.CoreAudioSignals.OnPlayRevolverTriggerSFX -= OnPlayRevolverTriggerSFX;

    private void OnPlayRevolverTriggerSFX()
        => revolverAudioSource.PlayOneShot(revolverTriggerClip);
}