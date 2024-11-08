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

        [SerializeField] private GameObject introElements;

        private void OnEnable()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            introElements.SetActive(false);
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
            introElements.SetActive(true);
            canvasGroup.DOFade(0, 0.75f).OnComplete(() =>
            {
                this.gameObject.SetActive(false);
            });
        }
    }
}