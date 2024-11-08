using System;
using UnityEngine;
using UnityEngine.UI;

namespace CrazyGames24
{
    public class PlayerBeat : MonoBehaviour
    {
        [Range(0, 1)] public float synchValue;
        public Slider synchSlider;


        [SerializeField] private float initialSynch = 0.3f;
        [SerializeField] private float beforeValue = -0.125f;
        [SerializeField] private float onTimeValue = 0.075f;
        [SerializeField] private float onMissedValue = -0.2f;

        Player player;

        private void Start()
        {
            player = GetComponent<Player>();

            player.OnAttachFish.AddListener(SetBeatdetector);
            player.OnDetachFish.AddListener(ReleaseBeatdetector);

            synchValue = initialSynch;
        }

        private void SetBeatdetector()
        {
            player.currentFish.beatDetector.BeatBefore += OnBefore;
            player.currentFish.beatDetector.BeatOnTime += OnTime;
            player.currentFish.beatDetector.BeatMissed += OnMissed;
        }

        private void ReleaseBeatdetector(Fish currentFish)
        {
            currentFish.beatDetector.BeatBefore -= OnBefore;
            currentFish.beatDetector.BeatOnTime -= OnTime;
            currentFish.beatDetector.BeatMissed -= OnMissed;
        }

        private void OnBefore(Prebeat prebeat)
        {
            AddSynch(beforeValue);
        }
        private void OnTime(Prebeat prebeat)
        {
            AddSynch(onTimeValue);
        }
        private void OnMissed(Prebeat prebeat)
        {
            AddSynch(onMissedValue);
        }


        private void AddSynch(float value)
        {
            synchValue += value;
            synchValue = Mathf.Max(0, Mathf.Min(1f, synchValue));
        }

        private void Update()
        {
            if (player.currentFish == null) return;

            // player.speed = Mathf.Min(0, Mathf.Lerp(-player.currentFish.speed, player.currentFish.speed, synchValue));
            player.speed = -player.currentFish.speed * Mathf.Cos(2 * Mathf.PI * synchValue);
            player.pullingSpeed = Mathf.Lerp(-5, 5, synchValue);

            synchSlider.value = synchValue;
        }
    }
}