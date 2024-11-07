using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace CrazyGames24
{
    public class Player : MonoBehaviour
    {
        [HideInInspector] public Rigidbody rb;
        [SerializeField] private LineRenderer fishingLine;
        [SerializeField] private Transform lineStartTransform;
        [SerializeField] private Transform lineMiddleTransform;
        [SerializeField] private Animator fishermanAnimator;
        [SerializeField] private CinemachineCamera cinemachineCamera;

        public Transform[] fishingSpotTransforms;

        public Fish currentFish;
        public bool isFishing = false;
        public float speed = 5f;
        public float pullingSpeed = 0f;

        public UnityEvent OnAttachFish;
        public UnityEvent<Fish> OnDetachFish;

        public void Start()
        {
            rb = GetComponent<Rigidbody>();
            GameManager.Instance.inputManager.OnTriggerBeatPerformed += CheckBeatOnFish;
        }

        public void OnDisable()
        {
            GameManager.Instance.inputManager.OnTriggerBeatPerformed -= CheckBeatOnFish;
        }

        private void CheckBeatOnFish()
        {
            if (currentFish == null) return;
            currentFish.beatDetector.CheckBeatStatus();
        }

        public void AttachFish(Fish targetFish, Action OnAnimationCompleted)
        {
            if (currentFish != null)
            {
                OnDetachFish?.Invoke(currentFish);
                currentFish.Detach();
            }
            currentFish = targetFish;
            cinemachineCamera.Follow = currentFish.transform;
            cinemachineCamera.LookAt = currentFish.transform;

            fishermanAnimator.SetTrigger("Cast");

            StartCoroutine(WaitAnimation(() =>
            {
                isFishing = true;
                OnAnimationCompleted?.Invoke();
                OnAttachFish?.Invoke();
            }));
        }

        public void DetachFish(Fish fish)
        {
            isFishing = false;
            currentFish = null;
            cinemachineCamera.Follow = this.transform;
            cinemachineCamera.LookAt = this.transform;
            fishingLine.SetPosition(0, lineStartTransform.position);
            fishingLine.SetPosition(1, lineMiddleTransform.position);
            fishingLine.SetPosition(2, lineMiddleTransform.position);

            GameManager.Instance.audioEventManager.Stop();
        }

        private IEnumerator WaitAnimation(Action callback)
        {
            yield return new WaitForSeconds(5f);

            callback.Invoke();
        }

        private void Update()
        {
            if (!isFishing) return;
            if (currentFish == null) return;
            fishingLine.SetPosition(0, lineStartTransform.position);
            fishingLine.SetPosition(1, lineMiddleTransform.position);
            fishingLine.SetPosition(2, currentFish.transform.position);

            transform.position = Vector3.Lerp(transform.position, transform.position + currentFish.transform.forward, Time.deltaTime * speed);
        }
    }
}