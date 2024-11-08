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

        Vector3 defaultPosition;
        Vector3 defaultRotation;

        [SerializeField] private int spotIndex = 0;

        [SerializeField] private AudioAnalysisDataSO audioDataSO;

        [Header("VFX")]
        [SerializeField] private ParticleSystem idleVFX;
        [SerializeField] private Renderer triggerVFX;
        [SerializeField] private Color[] colorsVFX;

        private void Start()
        {
            defaultSpeed = speed;

            triggerVFX.enabled = false;
            idleVFX.Play();

            defaultPosition = transform.localPosition;
            defaultRotation = transform.localEulerAngles;
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

            // transform.gameObject.SetActive(false);

            // transform.position = GameManager.Instance.player.fishingSpotTransforms[spotIndex].position;

            StartCoroutine(ResetFish());
        }

        private IEnumerator ResetFish()
        {
            yield return new WaitForSeconds(5f);

            transform.localEulerAngles = defaultRotation;
            transform.localPosition = new Vector3(defaultPosition.x, -2, defaultPosition.z);
            transform.DOLocalMoveY(defaultPosition.y, 0.25f);

            speed = defaultSpeed;
            triggerVFX.enabled = false;
            idleVFX.Play();
        }

        public void CollectFish()
        {
            triggerVFX.enabled = false;

            speed = 0;
            isAttached = false;
            GameManager.Instance.player.DetachFish(this, true);

            transform.gameObject.SetActive(false);
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

            if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < 5f) CollectFish();
            if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) > 35f) Detach();
        }
    }
}