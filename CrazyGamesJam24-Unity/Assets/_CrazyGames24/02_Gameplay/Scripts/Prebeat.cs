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
        Vector2 targetPosition;
        Vector2 hitPosition;
        bool isReady;

        private float elapsedTime = 0f;

        public float duration = 2f;

        RectTransform rt;
        Image beatImg;

        public BeatStatus beatStatus;

        [SerializeField] private float beatThreshold = 10f;

        [SerializeField] private Color[] statusColors;

        public bool isCleared = false;

        private void OnEnable()
        {
            rt = GetComponent<RectTransform>();
            beatImg = GetComponent<Image>();
        }

        public void Init(Vector2 target, Vector2 _hitPosition)
        {
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
            if (!isReady) return;

            if (rt.anchoredPosition.y > beatThreshold)
            {
                beatStatus = BeatStatus.Before;
            }
            else if (rt.anchoredPosition.y < -beatThreshold)
            {
                beatStatus = BeatStatus.After;
            }
            else
            {
                beatStatus = BeatStatus.OnTime;
            }

            beatImg.color = statusColors[(int)beatStatus];

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the progress (goes from 0 to 1 over the duration)
            float progress = Mathf.Clamp01(elapsedTime / duration);

            rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition, targetPosition, progress);

            if (progress >= 1f)
            {
                isReady = false;
            }
        }

        public void ClearBeat()
        {
            isCleared = true;
            beatStatus = BeatStatus.Cleared;
            beatImg.color = statusColors[(int)beatStatus];
        }
    }
}