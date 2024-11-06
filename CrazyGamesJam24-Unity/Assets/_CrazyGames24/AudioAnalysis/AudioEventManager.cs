using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public class DetectedAudioEvent : UnityEvent<float> { }
[System.Serializable]
public class PreAudioEvent : UnityEvent<float> { }
[System.Serializable]
public class FirstAudioEvent : UnityEvent { } // Event that triggers only for the first audio event

public class AudioEventManager : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioAnalysisDataSO analysisDataSO;

    public DetectedAudioEvent onAudioEventTriggered;
    public PreAudioEvent onPreAudioEventTriggered;
    public FirstAudioEvent onFirstAudioEventTriggered; // Event to trigger only once

    public float preEventOffset = 2.0f;

    private List<AudioEvent> pendingEvents = new List<AudioEvent>();
    private int eventIndex = 0;
    private int preEventIndex = 0;
    private bool isPlaying = false;
    private float currentTime = 0f;
    private bool firstEventTriggered = false; // Flag to ensure the first event triggers only once

    [SerializeField] private bool verbose = false;
    [SerializeField] private bool loop = false;

    private void Start()
    {
        if (audioClip == null || analysisDataSO == null)
        {
            Debug.LogError("AudioClip or AudioAnalysisDataSO is not assigned.");
            return;
        }

        pendingEvents = new List<AudioEvent>(analysisDataSO.events);
        eventIndex = 0;
        preEventIndex = 0;
        currentTime = 0f;
        firstEventTriggered = false; // Reset the flag at the start
    }

    private void Update()
    {
        if (!isPlaying) return;

        currentTime += Time.deltaTime;

        // Trigger pre-events a few seconds before each main event
        while (preEventIndex < pendingEvents.Count && pendingEvents[preEventIndex].timestamp - preEventOffset <= currentTime)
        {
            TriggerPreEvent(pendingEvents[preEventIndex]);
            preEventIndex++;
        }

        // Check for main events that need to be triggered at the current time
        while (eventIndex < pendingEvents.Count && pendingEvents[eventIndex].timestamp <= currentTime)
        {
            TriggerEvent(pendingEvents[eventIndex]);
            eventIndex++;
        }

        if (currentTime >= audioClip.length)
        {
            isPlaying = false;

            if (loop) Play();
        }
    }

    private void TriggerEvent(AudioEvent audioEvent)
    {
        // If this is the first event being triggered, invoke the first-event UnityEvent
        if (!firstEventTriggered)
        {
            onFirstAudioEventTriggered.Invoke();
            firstEventTriggered = true; // Set the flag to prevent future triggers
        }

        onAudioEventTriggered.Invoke(audioEvent.amplitude);
        if (verbose) Debug.Log($"Triggered audio event at {audioEvent.timestamp} seconds with amplitude {audioEvent.amplitude}");
    }

    private void TriggerPreEvent(AudioEvent audioEvent)
    {
        onPreAudioEventTriggered.Invoke(audioEvent.amplitude);
        if (verbose) Debug.Log($"Triggered pre-event {preEventOffset} seconds before {audioEvent.timestamp} with amplitude {audioEvent.amplitude}");
    }

    public void Play()
    {
        currentTime = 0f;
        eventIndex = 0;
        preEventIndex = 0;
        isPlaying = true;
        firstEventTriggered = false; // Reset the flag in case of replay
    }

    public void Stop()
    {
        isPlaying = false;
        currentTime = 0f;
        eventIndex = 0;
        preEventIndex = 0;
    }

    public void Pause()
    {
        isPlaying = false;
    }
}
