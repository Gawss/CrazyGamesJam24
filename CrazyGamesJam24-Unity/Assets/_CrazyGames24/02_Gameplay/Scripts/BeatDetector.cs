using System;
using UnityEngine;

namespace CrazyGames24
{
    public class BeatDetector : MonoBehaviour
    {
        public AudioSource audioSource;
        public float sensitivity = 1.5f;
        private float[] samples = new float[64];
        private float[] previousSamples = new float[64];

        public Action OnDrumDetected;
        public Action OnTransientDetected;
        public Action OnHitDetected;
        public Action OnNothing;

        void Update()
        {
            audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);

            // Detect low-frequency peaks (for bass drum)
            float lowFreqAmplitude = 0;
            for (int i = 1; i < 5; i++) // Adjust indices for your frequency range
            {
                lowFreqAmplitude += samples[i];
            }

            // Detect high-frequency peaks (for hi-hats)
            float highFreqAmplitude = 0;
            for (int i = 30; i < 50; i++) // Adjust indices for high frequencies
            {
                highFreqAmplitude += samples[i];
            }

            // Calculate Spectral Flux
            float spectralFlux = 0;
            for (int i = 0; i < samples.Length; i++)
            {
                float flux = samples[i] - previousSamples[i];
                spectralFlux += flux > 0 ? flux : 0; // Only consider positive changes
            }

            // Trigger events based on thresholds
            if (lowFreqAmplitude > sensitivity)
            {
                Debug.Log("Bass Drum Detected");
                OnDrumDetected?.Invoke();
            }
            else if (highFreqAmplitude > sensitivity)
            {
                Debug.Log("Hi-Hat or Cymbal Detected");
                OnHitDetected?.Invoke();
            }
            else if (spectralFlux > sensitivity)
            {
                Debug.Log("Transient Detected");
                OnTransientDetected?.Invoke();
            }
            else
            {
                OnNothing?.Invoke();
            }

            // Update previous samples for the next frame
            System.Array.Copy(samples, previousSamples, samples.Length);
        }
    }
}