using System;
using UnityEngine;

namespace CrazyGames24
{
    public class BeatDetector : MonoBehaviour
    {
        Prebeat lastBeatDetected;

        public Action<Prebeat> BeatOnTime;
        public Action<Prebeat> BeatAfter;
        public Action<Prebeat> BeatBefore;
        public Action<Prebeat> BeatMissed;

        private void Start()
        {
            GameManager.Instance.inputManager.OnTriggerBeatPerformed += CheckBeatStatus;
        }

        private void OnDestroy()
        {
            GameManager.Instance.inputManager.OnTriggerBeatPerformed -= CheckBeatStatus;
        }

        private void CheckBeatStatus()
        {
            if (lastBeatDetected == null) return;

            if (lastBeatDetected.beatStatus == BeatStatus.Before) BeatAfter?.Invoke(lastBeatDetected);
            if (lastBeatDetected.beatStatus == BeatStatus.OnTime)
            {
                lastBeatDetected.ClearBeat();
                BeatOnTime?.Invoke(lastBeatDetected);
            }
            if (lastBeatDetected.beatStatus == BeatStatus.After) BeatBefore?.Invoke(lastBeatDetected);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Prebeat beat))
            {
                lastBeatDetected = beat;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Prebeat beat))
            {
                if (!beat.isCleared) BeatMissed?.Invoke(beat);
            }
        }
    }
}