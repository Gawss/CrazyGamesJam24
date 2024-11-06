using System;
using UnityEngine;

namespace CrazyGames24
{
    public class PlayerBeat : MonoBehaviour
    {
        [Range(0, 1)] public float synchValue;

        Player player;

        private void Start()
        {
            player = GetComponent<Player>();

            GameManager.Instance.beatDetector.BeatBefore += OnBefore;
            GameManager.Instance.beatDetector.BeatOnTime += OnTime;
            GameManager.Instance.beatDetector.BeatMissed += OnMissed;

            synchValue = 0.5f;
        }


        private void OnDestroy()
        {
            GameManager.Instance.beatDetector.BeatBefore -= OnBefore;
            GameManager.Instance.beatDetector.BeatOnTime -= OnTime;
            GameManager.Instance.beatDetector.BeatMissed -= OnMissed;
        }

        private void OnBefore(Prebeat prebeat)
        {
            AddSynch(-0.125f);
        }
        private void OnTime(Prebeat prebeat)
        {
            AddSynch(0.075f);
        }
        private void OnMissed(Prebeat prebeat)
        {
            AddSynch(-0.2f);
        }


        private void AddSynch(float value)
        {
            synchValue += value;
            synchValue = Mathf.Max(0, Mathf.Min(1, synchValue));
        }

        private void Update()
        {
            if (player.currentFish == null) return;

            // player.speed = Mathf.Min(0, Mathf.Lerp(-player.currentFish.speed, player.currentFish.speed, synchValue));
            player.speed = -player.currentFish.speed * Mathf.Cos(2 * Mathf.PI * synchValue);
            player.pullingSpeed = Mathf.Lerp(-5, 5, synchValue);
        }
    }
}