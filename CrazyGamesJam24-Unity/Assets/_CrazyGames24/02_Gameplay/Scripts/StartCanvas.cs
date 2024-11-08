using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CrazyGames24
{
    public class StartCanvas : MonoBehaviour
    {
        [SerializeField] private Button mainButton;

        CanvasGroup canvasGroup;

        private void OnEnable()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.75f).OnComplete(() =>
            {
                mainButton.onClick.AddListener(OnMainButtonClick);
            });
        }

        private void OnDisable()
        {
            mainButton.onClick.RemoveListener(OnMainButtonClick);
        }

        private void OnMainButtonClick()
        {
            GameManager.Instance.GameRunning = true;
            canvasGroup.DOFade(0, 0.75f).OnComplete(() =>
            {
                this.gameObject.SetActive(false);
            });
        }
    }
}