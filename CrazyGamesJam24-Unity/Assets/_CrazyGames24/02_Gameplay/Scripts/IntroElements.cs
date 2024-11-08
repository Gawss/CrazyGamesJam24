using DG.Tweening;
using StylizedWater2;
using UnityEngine;

namespace CrazyGames24
{
    public class IntroElements : MonoBehaviour
    {
        [SerializeField] private AlignToWaves alignToWaves;
        [SerializeField] private Lake firstLake;
        [SerializeField] private IngameCanvas ingameCanvas;
        private void OnEnable()
        {
            if (GameManager.Instance == null) return;

            alignToWaves.enabled = true;
            firstLake.gameObject.SetActive(false);
            transform.localPosition += Vector3.up * -6f;

            float height = 0;

            DOTween.To(() => height, x => height = x, 1.4f, 3.5f).OnUpdate(() => { alignToWaves.heightOffset = height; }).OnComplete(() =>
            {
                GameManager.Instance.inputManager.OnTriggerBeatPerformed += HideElements;
            });
        }

        private void HideElements()
        {
            GameManager.Instance.inputManager.OnTriggerBeatPerformed -= HideElements;

            GameManager.Instance.GameRunning = true;
            firstLake.gameObject.SetActive(true);
            ingameCanvas.gameObject.SetActive(true);

            float height = 1.4f;

            DOTween.To(() => height, x => height = x, -2f, 3.5f).OnUpdate(() => { alignToWaves.heightOffset = height; });
        }
    }
}