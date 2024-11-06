using System;
using CrazyGames24;
using UnityEngine;
using UnityEngine.UI;

public class BeatInterface : MonoBehaviour
{
    [SerializeField] private Image beatImg;
    [SerializeField] private GameObject beatPrefab;
    [SerializeField] private Transform beatInitialTransform;
    [SerializeField] private Transform beatFinalTransform;

    [SerializeField] private Image[] beatFeedback;

    Prebeat currentBeat;

    private void Start()
    {
        GameManager.Instance.beatDetector.BeatBefore += OnBeat;
        GameManager.Instance.beatDetector.BeatOnTime += OnBeat;
        GameManager.Instance.beatDetector.BeatAfter += OnBeat;

        foreach (var b in beatFeedback) b.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.beatDetector.BeatBefore -= OnBeat;
        GameManager.Instance.beatDetector.BeatOnTime -= OnBeat;
        GameManager.Instance.beatDetector.BeatAfter -= OnBeat;
    }

    private void OnBeat(Prebeat prebeat)
    {
        foreach (var b in beatFeedback) b.gameObject.SetActive(false);

        beatFeedback[(int)prebeat.beatStatus].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (currentBeat == null) return;
    }

    public void OnAudioEvent(float amplitude)
    {
        beatImg.color = new Color(UnityEngine.Random.Range(0f, 1f),
                                UnityEngine.Random.Range(0f, 1f),
                                UnityEngine.Random.Range(0f, 1f), 1f);
    }

    public void OnPreEvent(float amplitude)
    {
        GameObject bGO = Instantiate(beatPrefab, beatImg.transform.parent);
        bGO.GetComponent<RectTransform>().anchoredPosition = beatInitialTransform.GetComponent<RectTransform>().anchoredPosition;

        // bGO.GetComponent<Image>().color = beatImg.color;
        bGO.GetComponent<Prebeat>().Init(beatFinalTransform.GetComponent<RectTransform>().anchoredPosition, beatImg.GetComponent<RectTransform>().anchoredPosition);

        currentBeat = bGO.GetComponent<Prebeat>();
    }
}
