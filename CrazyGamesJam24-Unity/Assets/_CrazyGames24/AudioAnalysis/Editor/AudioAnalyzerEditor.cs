using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AudioAnalyzerEditor : EditorWindow
{
    private AudioClip audioClip;
    private AudioAnalysisDataSO analysisDataSO;

    private bool useAutomaticThreshold = false; // Checkbox to enable automatic threshold
    private float threshold = 0.5f; // Manual threshold value
    private float minDrumInterval = 0.75f;

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
            // Show manual threshold slider only when not using automatic threshold
            threshold = EditorGUILayout.Slider("Threshold", threshold, 0f, 1f);
        }

        minDrumInterval = EditorGUILayout.FloatField("Min Drum Interval (s)", minDrumInterval);

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

        // Calculate threshold if using automatic mode
        if (useAutomaticThreshold)
        {
            float averageAmplitude = 0f;
            for (int i = 0; i < samples.Length; i++)
            {
                averageAmplitude += Mathf.Abs(samples[i]);
            }
            averageAmplitude /= samples.Length;

            threshold = averageAmplitude * 1.5f; // Adjust multiplier as needed for sensitivity
            Debug.Log($"Automatic threshold set to {threshold}");
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

        EditorUtility.SetDirty(analysisDataSO); // Mark the ScriptableObject as dirty so it saves changes
        AssetDatabase.SaveAssets();
    }
}
