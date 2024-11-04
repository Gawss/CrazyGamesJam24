using UnityEngine;

namespace CrazyGames24
{
    public class Prebeat : MonoBehaviour
    {
        Vector3 targetPosition;
        bool isReady;

        private float elapsedTime = 0f;

        public float duration = 2f;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Init(Vector3 target)
        {
            targetPosition = target;
            elapsedTime = 0f;
            isReady = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isReady) return;

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the progress (goes from 0 to 1 over the duration)
            float progress = Mathf.Clamp01(elapsedTime / duration);

            transform.position = Vector3.Lerp(transform.position, targetPosition, progress);

            if (progress >= 1f)
            {
                isReady = false;
            }
        }
    }
}