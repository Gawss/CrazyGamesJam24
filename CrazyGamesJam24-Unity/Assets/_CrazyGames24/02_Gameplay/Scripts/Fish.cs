using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;

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
        [SerializeField] private Renderer triggerVFX;
        [SerializeField] private Color[] colorsVFX;

        private void OnEnable()
        {
            defaultSpeed = speed;

            triggerVFX.enabled = false;
            idleVFX.Play();
        }

        Tween colorTween;

        public void SetTriggerColor(int _colorIndex)
        {
            colorTween?.Kill();
            colorTween = triggerVFX.material.DOColor(colorsVFX[_colorIndex], 0.15f).SetLoops(2, LoopType.Yoyo);
        }

        private IEnumerator ResetColor()
        {
            yield return new WaitForSeconds(0.75f);
            SetTriggerColor(0);
        }

        public void Attach()
        {

            Vector3 directionToTarget = GameManager.Instance.player.transform.position - transform.position;
            Vector3 oppositeDirection = -directionToTarget;
            Vector3 lookAwayPosition = transform.position + oppositeDirection;

            GameManager.Instance.audioEventManager.audioClip = audioDataSO.audioClip;
            GameManager.Instance.audioEventManager.analysisDataSO = audioDataSO;

            lookAwayPosition.y = transform.position.y;


            SetTriggerColor(0);
            triggerVFX.enabled = true;

            idleVFX.Stop();
            idleVFX.Clear();

            transform.LookAt(lookAwayPosition, transform.up);

            speed = defaultSpeed;
            GameManager.Instance.player.AttachFish(this, () =>
            {
                isAttached = true;
            });

        }

        public void Detach()
        {
            triggerVFX.enabled = false;

            speed = 0;
            isAttached = false;
            GameManager.Instance.player.DetachFish(this);

            transform.gameObject.SetActive(false);

            // transform.position = GameManager.Instance.player.fishingSpotTransforms[spotIndex].position;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!GameManager.Instance.GameRunning) return;
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