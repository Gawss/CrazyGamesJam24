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

        [SerializeField] private GameObject fishingBar;
        [SerializeField] private GameObject paddle;
        [SerializeField] private Animator fishermanAnimator;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private CameraTarget cameraTarget;

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

            SetCharacter(false);
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

        public void SetCharacter(bool isMoving)
        {
            fishermanAnimator.SetBool("isMoving", isMoving);

            fishingBar.SetActive(!isMoving);
            paddle.SetActive(isMoving);
        }

        public void AttachFish(Fish targetFish, Action OnAnimationCompleted)
        {
            if (currentFish != null)
            {
                OnDetachFish?.Invoke(currentFish);
                currentFish.Detach();
            }
            currentFish = targetFish;

            fishermanAnimator.SetTrigger("Cast");

            StartCoroutine(WaitAnimation(() =>
            {
                isFishing = true;
                SetCharacter(false);
                OnAnimationCompleted?.Invoke();
                OnAttachFish?.Invoke();
            }));
        }

        public void DetachFish(Fish fish)
        {
            isFishing = false;
            currentFish = null;

            cameraTarget.SetTarget(this.transform);

            fishingLine.SetPosition(0, lineStartTransform.position);
            fishingLine.SetPosition(1, lineMiddleTransform.position);
            fishingLine.SetPosition(2, lineMiddleTransform.position);

            GameManager.Instance.audioEventManager.Stop();
        }

        private IEnumerator WaitAnimation(Action callback)
        {
            yield return new WaitForSeconds(2.5f);

            cameraTarget.SetTarget(currentFish.transform);

            yield return new WaitForSeconds(2.5f);

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