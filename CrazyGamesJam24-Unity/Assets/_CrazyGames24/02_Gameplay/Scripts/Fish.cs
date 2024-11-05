using UnityEngine;
using UnityEngine.EventSystems;

namespace CrazyGames24
{
    public class Fish : MonoBehaviour, IPointerDownHandler
    {
        bool isAttached = false;
        public float speed = 5f;
        float defaultSpeed;

        private void OnEnable()
        {
            defaultSpeed = speed;
        }

        public void Attach()
        {

            Vector3 directionToTarget = GameManager.Instance.player.transform.position - transform.position;
            Vector3 oppositeDirection = -directionToTarget;
            Vector3 lookAwayPosition = transform.position + oppositeDirection;

            transform.LookAt(lookAwayPosition);

            GameManager.Instance.player.AttachFish(this);

            speed = defaultSpeed;
            isAttached = true;
        }

        public void Deattach()
        {
            this.transform.SetParent(null, true);

            speed = 0;
            isAttached = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isAttached) Deattach();
            else Attach();
        }

        private void Update()
        {
            if (!isAttached) return;

            transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * (1f - GameManager.Instance.player.pullingSpeed), Time.deltaTime * speed);
        }
    }
}