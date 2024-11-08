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

            alignToWaves.enabled = false;

            firstLake.gameObject.SetActive(false);
            transform.localPosition += Vector3.up * -6f;
            transform.DOMoveY(0.25f, 3.5f).OnComplete(() =>
            {
                alignToWaves.enabled = true;
                GameManager.Instance.inputManager.OnTriggerBeatPerformed += HideElements;
            });
        }

        private void HideElements()
        {
            GameManager.Instance.inputManager.OnTriggerBeatPerformed -= HideElements;
            alignToWaves.enabled = false;

            GameManager.Instance.GameRunning = true;
            firstLake.gameObject.SetActive(true);
            ingameCanvas.gameObject.SetActive(true);
            transform.DOMoveY(-6f, 2.5f);
        }
    }
}