using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AudioAnalyzerEditor : EditorWindow
{
    private AudioClip audioClip;
    private AudioAnalysisDataSO analysisDataSO;

    private bool useAutomaticThreshold = false;
    private bool useAutomaticMinInterval = false; // Checkbox for automatic minDrumInterval
    private float threshold = 0.5f;
    private float minDrumInterval = 0.75f;

    // New fields for note size and speed
    private float noteSize = 1f;
    private float noteSpeed = 5f;

    [MenuItem("Tools/Audio Analyzer")]
    public static void ShowWindow()
    {
        GetWindow<AudioAnalyzerEditor>("Audio Analyzer");
    }

    void OnGUI()
    {
        GUILayout.Label("Audio Analysis Tool", EditorStyles.boldLabel);

        audioClip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", audioClip, typeof(AudioClip), false);
        analysisDataSO = (AudioAnalysisDataSO)EditorGUILayout.ObjectField("Analysis Data SO", analysisDataSO, typeof(AudioAnalysisDataSO), false);

        useAutomaticThreshold = EditorGUILayout.Toggle("Use Automatic Threshold", useAutomaticThreshold);
        if (!useAutomaticThreshold)
        {
            threshold = EditorGUILayout.Slider("Threshold", threshold, 0f, 1f);
        }

        useAutomaticMinInterval = EditorGUILayout.Toggle("Use Automatic Min Drum Interval", useAutomaticMinInterval);
        if (!useAutomaticMinInterval)
        {
            minDrumInterval = EditorGUILayout.FloatField("Min Drum Interval (s)", minDrumInterval);
        }

        // Inputs for note size and speed
        noteSize = EditorGUILayout.FloatField("Note Size", noteSize);
        noteSpeed = EditorGUILayout.FloatField("Note Speed", noteSpeed);

        if (GUILayout.Button("Analyze Audio"))
        {
            if (audioClip == null)
            {
                Debug.LogError("Please assign an AudioClip to analyze.");
                return;
            }

            if (analysisDataSO == null)
            {
                CreateNewAnalysisDataAsset();
            }

            AnalyzeAudio();
            Debug.Log("Audio analysis complete.");
        }
    }

    private void CreateNewAnalysisDataAsset()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Analysis Data", "NewAudioAnalysisData", "asset", "Please enter a file name to save the analysis data.");
        if (!string.IsNullOrEmpty(path))
        {
            analysisDataSO = ScriptableObject.CreateInstance<AudioAnalysisDataSO>();
            AssetDatabase.CreateAsset(analysisDataSO, path);
            AssetDatabase.SaveAssets();
            Debug.Log("Created new AudioAnalysisDataSO at " + path);
        }
    }

    private void AnalyzeAudio()
    {
        analysisDataSO.audioClip = audioClip;
        analysisDataSO.clipLength = audioClip.length;
        analysisDataSO.events.Clear();

        float[] samples = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(samples, 0);

        if (useAutomaticThreshold)
        {
            float averageAmplitude = 0f;
            for (int i = 0; i < samples.Length; i++)
            {
                averageAmplitude += Mathf.Abs(samples[i]);
            }
            averageAmplitude /= samples.Length;
            threshold = averageAmplitude * 1.5f;
            Debug.Log($"Automatic threshold set to {threshold}");
        }

        if (useAutomaticMinInterval)
        {
            minDrumInterval = EstimateMinDrumInterval(samples, audioClip.frequency, noteSize, noteSpeed);
            Debug.Log($"Automatic min drum interval set to {minDrumInterval}s");
        }

        float lastDrumTime = -minDrumInterval;

        for (int i = 0; i < samples.Length; i++)
        {
            float amplitude = Mathf.Abs(samples[i]);
            float currentTime = (float)i / audioClip.frequency;

            if (amplitude > threshold && currentTime - lastDrumTime >= minDrumInterval)
            {
                AudioEvent audioEvent = new AudioEvent
                {
                    timestamp = currentTime,
                    amplitude = amplitude
                };
                analysisDataSO.events.Add(audioEvent);
                lastDrumTime = currentTime;
            }
        }

        EditorUtility.SetDirty(analysisDataSO);
        AssetDatabase.SaveAssets();
    }

    private float EstimateMinDrumInterval(float[] samples, int sampleRate, float noteSize, float noteSpeed)
    {
        float averagePeakInterval = 0f;
        List<float> peakIntervals = new List<float>();
        float lastPeakTime = -1f;

        for (int i = 0; i < samples.Length; i++)
        {
            float amplitude = Mathf.Abs(samples[i]);
            float currentTime = (float)i / sampleRate;

            if (amplitude > threshold)
            {
                if (lastPeakTime >= 0)
                {
                    float interval = currentTime - lastPeakTime;
                    peakIntervals.Add(interval);
                }
                lastPeakTime = currentTime;
            }
        }

        if (peakIntervals.Count > 0)
        {
            foreach (float interval in peakIntervals)
            {
                averagePeakInterval += interval;
            }
            averagePeakInterval /= peakIntervals.Count;
        }

        float requiredTimeForSpacing = noteSize / noteSpeed;

        return Mathf.Max(averagePeakInterval * 0.5f, requiredTimeForSpacing, 0.1f);
    }
}
