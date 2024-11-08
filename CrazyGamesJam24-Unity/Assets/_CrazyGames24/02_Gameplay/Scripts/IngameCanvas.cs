using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CrazyGames24
{
    public class IngameCanvas : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        [SerializeField] private TextMeshProUGUI songsCollectedScore;

        private void OnEnable()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.75f);

        }

        private void Start()
        {
            GameManager.Instance.player.OnSongCollected += OnSongCollected;
            OnSongCollected();
        }

        private void OnDestroy()
        {
            GameManager.Instance.player.OnSongCollected -= OnSongCollected;
        }

        private void OnSongCollected()
        {
            songsCollectedScore.text = $"{GameManager.Instance.player.songsCollected}";
        }
    }
}