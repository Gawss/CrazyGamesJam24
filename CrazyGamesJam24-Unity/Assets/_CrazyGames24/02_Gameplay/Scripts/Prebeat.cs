using UnityEngine;
using UnityEngine.UI;

namespace CrazyGames24
{
    public enum BeatStatus
    {
        Before,
        OnTime,
        After,
        Cleared
    }
    public class Prebeat : MonoBehaviour
    {
        Vector3 startPosition;
        Vector3 targetPosition;
        Vector3 hitPosition;
        bool isReady;

        private float elapsedTime = 0f;

        public float duration = 2f;
        [SerializeField] private Renderer beatImg;

        public BeatStatus beatStatus;

        [SerializeField] private float beatThreshold = 10f;

        [SerializeField] private Color[] statusColors;

        [SerializeField] private ParticleSystem redVFX;

        public bool isCleared = false;

        public void Init(Vector3 target, Vector3 _hitPosition)
        {
            startPosition = transform.localPosition;
            hitPosition = _hitPosition;
            targetPosition = target;
            elapsedTime = 0f;
            beatStatus = BeatStatus.Before;
            isCleared = false;
            isReady = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (isCleared) return;
            if (!isReady) return;

            if (transform.localPosition.z > beatThreshold)
            {
                beatStatus = BeatStatus.Before;
            }
            else if (transform.localPosition.z < -beatThreshold)
            {
                beatStatus = BeatStatus.After;
            }
            else
            {
                beatStatus = BeatStatus.OnTime;
            }

            // beatImg.materials[0].color = statusColors[(int)beatStatus];

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the progress (goes from 0 to 1 over the duration)
            float progress = Mathf.Clamp01(elapsedTime / duration);

            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);

            if (progress >= 1f)
            {
                isReady = false;
                redVFX.Play();
                beatImg.enabled = false;
            }
        }

        public void ClearBeat()
        {
            isCleared = true;
            beatStatus = BeatStatus.Cleared;
            // beatImg.material.color = statusColors[(int)beatStatus];
            beatImg.enabled = false;
            redVFX.Play();
        }
    }
}