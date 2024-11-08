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

        [SerializeField] private AudioAnalysisDataSO audioDataSO;

        [Header("VFX")]
        [SerializeField] private ParticleSystem idleVFX;
        [SerializeField] private ParticleSystem[] beatTriggersVFX;

        private void OnEnable()
        {
            defaultSpeed = speed;

            foreach (var vfx in beatTriggersVFX) vfx.gameObject.SetActive(false);
            beatTriggersVFX[0].gameObject.SetActive(true);
        }

        public void SetTriggerColor(int _colorIndex)
        {
            foreach (var vfx in beatTriggersVFX) vfx.gameObject.SetActive(false);
            beatTriggersVFX[_colorIndex].gameObject.SetActive(true);
        }

        public void Attach()
        {

            Vector3 directionToTarget = GameManager.Instance.player.transform.position - transform.position;
            Vector3 oppositeDirection = -directionToTarget;
            Vector3 lookAwayPosition = transform.position + oppositeDirection;

            GameManager.Instance.audioEventManager.audioClip = audioDataSO.audioClip;
            GameManager.Instance.audioEventManager.analysisDataSO = audioDataSO;

            lookAwayPosition.y = transform.position.y;

            beatTriggersVFX[0].gameObject.SetActive(true);

            transform.LookAt(lookAwayPosition, transform.up);

            speed = defaultSpeed;
            GameManager.Instance.player.AttachFish(this, () =>
            {
                isAttached = true;
            });

        }

        public void Detach()
        {
            foreach (var vfx in beatTriggersVFX) vfx.gameObject.SetActive(false);
            speed = 0;
            isAttached = false;
            GameManager.Instance.player.DetachFish(this);

            transform.gameObject.SetActive(false);

            // transform.position = GameManager.Instance.player.fishingSpotTransforms[spotIndex].position;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isAttached) Detach();
            else Attach();
        }

        private void Update()
        {
            if (!isAttached) return;

            Vector3 directionToTarget = GameManager.Instance.player.transform.position - transform.position;
            Vector3 oppositeDirection = -directionToTarget;
            Vector3 lookAwayPosition = transform.position + oppositeDirection;

            lookAwayPosition.y = transform.position.y;

            transform.LookAt(lookAwayPosition, transform.up);

            transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * (1f - GameManager.Instance.player.pullingSpeed), Time.deltaTime * speed);

            if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < 5f) Detach();
            if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) > 35f) Detach();
        }
    }
}