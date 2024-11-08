using DG.Tweening;
using StylizedWater2;
using UnityEngine;

namespace CrazyGames24
{
    public class IntroElements : MonoBehaviour
    {
        [SerializeField] private AlignToWaves alignToWaves;
        private void OnEnable()
        {
            if (GameManager.Instance == null) return;

            alignToWaves.enabled = false;
            transform.localPosition += Vector3.up * -6f;
            transform.DOMoveY(0, 3.5f).OnComplete(() =>
            {
                alignToWaves.enabled = true;
                GameManager.Instance.inputManager.OnTriggerBeatPerformed += HideElements;
            });
        }

        private void HideElements()
        {
            GameManager.Instance.inputManager.OnTriggerBeatPerformed -= HideElements;
            transform.DOMoveY(-6f, 3.5f).OnComplete(() =>
            {

            });
        }
    }
}