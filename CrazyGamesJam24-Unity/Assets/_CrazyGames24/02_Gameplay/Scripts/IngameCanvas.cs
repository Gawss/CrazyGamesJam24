using DG.Tweening;
using UnityEngine;

namespace CrazyGames24
{
    public class IngameCanvas : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        private void OnEnable()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.75f);
        }
    }
}