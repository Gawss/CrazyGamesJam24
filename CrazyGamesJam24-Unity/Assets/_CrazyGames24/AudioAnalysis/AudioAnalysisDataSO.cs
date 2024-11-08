using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AudioAnalysisData", menuName = "Audio/Analysis Data")]
public class AudioAnalysisDataSO : ScriptableObject
{
    public AudioClip audioClip;
    public float clipLength;
    public List<AudioEvent> events = new List<AudioEvent>();
}
