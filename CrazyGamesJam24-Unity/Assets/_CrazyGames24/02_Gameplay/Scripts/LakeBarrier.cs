using System;
using DG.Tweening;
using UnityEngine;

namespace CrazyGames24
{
    public class LakeBarrier : MonoBehaviour
    {
        [SerializeField] private Collider barrierCollider;
        [SerializeField] private ParticleSystem barrierVFX;
        [SerializeField] private int requiredSongs = 1;
        [SerializeField] private CanvasGroup canvasGroup;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            barrierVFX.Play();

            if (GameManager.Instance == null) return;

            GameManager.Instance.player.OnSongCollected += CheckScore;

            canvasGroup.alpha = 1;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance == null) return;

            GameManager.Instance.player.OnSongCollected -= CheckScore;
        }

        private void CheckScore()
        {
            if (GameManager.Instance.player.songsCollected == requiredSongs)
            {
                if (barrierVFX.isPlaying) barrierVFX.Stop();
                barrierCollider.enabled = false;

                GameManager.Instance.player.cameraTarget.SetTarget(this.transform);
                canvasGroup.DOFade(0, 1f).SetDelay(5f).OnComplete(() =>
                {
                    GameManager.Instance.player.cameraTarget.SetTarget(GameManager.Instance.player.transform);
                });
            }
        }
    }
}