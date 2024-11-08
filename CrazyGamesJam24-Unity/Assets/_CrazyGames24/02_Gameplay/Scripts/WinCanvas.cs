using System;
using DG.Tweening;
using UnityEngine;

namespace CrazyGames24
{
    public class WinCanvas : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            GameManager.Instance.player.OnVictory += ShowCanvas;
        }

        private void OnDestroy()
        {
            GameManager.Instance.player.OnVictory -= ShowCanvas;
        }


        private void ShowCanvas()
        {
            canvasGroup.DOFade(1, 0.75f);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }
}