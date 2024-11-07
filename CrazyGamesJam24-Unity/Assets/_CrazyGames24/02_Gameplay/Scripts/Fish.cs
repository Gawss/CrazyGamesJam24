using UnityEngine;
using UnityEngine.EventSystems;

namespace CrazyGames24
{
    public class Fish : MonoBehaviour, IPointerDownHandler
    {
        public Transform beatInitialTransform;
        public Transform beatFinalTransform;
        public BeatDetector beatDetector;

        bool isAttached = false;
        public float speed = 5f;
        float defaultSpeed;

        [SerializeField] private int spotIndex = 0;

        private void OnEnable()
        {
            defaultSpeed = speed;
        }

        public void Attach()
        {

            Vector3 directionToTarget = GameManager.Instance.player.transform.position - transform.position;
            Vector3 oppositeDirection = -directionToTarget;
            Vector3 lookAwayPosition = transform.position + oppositeDirection;

            lookAwayPosition.y = transform.position.y;

            transform.LookAt(lookAwayPosition, transform.up);

            speed = defaultSpeed;
            GameManager.Instance.player.AttachFish(this, () =>
            {
                isAttached = true;
            });

        }

        public void Detach()
        {
            speed = 0;
            isAttached = false;
            GameManager.Instance.player.DetachFish(this);

            transform.position = GameManager.Instance.player.fishingSpotTransforms[spotIndex].position;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isAttached) Detach();
            else Attach();
        }

        private void Update()
        {
            if (!isAttached) return;

            transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * (1f - GameManager.Instance.player.pullingSpeed), Time.deltaTime * speed);

            if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < 5f) Detach();
            if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) > 35f) Detach();
        }
    }
}