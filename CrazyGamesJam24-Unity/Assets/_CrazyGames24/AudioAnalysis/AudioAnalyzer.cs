using UnityEngine;
using System.Collections.Generic;

public class AudioAnalyzer : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioAnalysisDataSO analysisDataSO; // Reference to the ScriptableObject asset

    void Start()
    {
        if (audioClip == null)
        {
            Debug.LogError("No audio clip assigned!");
            return;
        }

        if (analysisDataSO == null)
        {
            Debug.LogError("No AudioAnalysisDataSO assigned! Please create one in the Assets folder.");
            return;
        }

        analysisDataSO.clipLength = audioClip.length;
        analysisDataSO.events.Clear(); // Clear previous data to start fresh

        AnalyzeAudio();
        Debug.Log("Audio analysis complete and saved to ScriptableObject.");
    }

    private void AnalyzeAudio()
    {
        float[] samples = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(samples, 0);

        float threshold = 0.1f;  // Adjust this threshold for peak detection sensitivity
        float minDrumInterval = 0.1f;  // Minimum interval (in seconds) between drum hits

        float lastDrumTime = -minDrumInterval;

        for (int i = 0; i < samples.Length; i++)
        {
            float amplitude = Mathf.Abs(samples[i]);

            if (amplitude > threshold)
            {
                float currentTime = (float)i / audioClip.frequency;

                if (currentTime - lastDrumTime >= minDrumInterval)
                {
                    // Create and add a new AudioEvent
                    AudioEvent audioEvent = new AudioEvent
                    {
                        timestamp = currentTime,
                        amplitude = amplitude
                    };
                    analysisDataSO.events.Add(audioEvent);
                    lastDrumTime = currentTime;
                }
            }
        }
    }
}
