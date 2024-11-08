using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrazyGames24
{
    public class IngameCanvas : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        [SerializeField] private TextMeshProUGUI songsCollectedScore;

        [SerializeField] private Image[] lifePoints;

        private void OnEnable()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.75f);

            foreach (var lp in lifePoints) lp.enabled = true;

        }

        private void Start()
        {
            GameManager.Instance.player.OnSongCollected += OnSongCollected;
            GameManager.Instance.player.OnLifePointsChanged += OnLifePoints;
            OnSongCollected();
        }

        private void OnLifePoints()
        {
            lifePoints[GameManager.Instance.player.lifePoints].enabled = false;
        }

        private void OnDestroy()
        {
            GameManager.Instance.player.OnSongCollected -= OnSongCollected;
            GameManager.Instance.player.OnLifePointsChanged -= OnLifePoints;
        }

        private void OnSongCollected()
        {
            songsCollectedScore.text = $"{GameManager.Instance.player.songsCollected}";
        }
    }
}