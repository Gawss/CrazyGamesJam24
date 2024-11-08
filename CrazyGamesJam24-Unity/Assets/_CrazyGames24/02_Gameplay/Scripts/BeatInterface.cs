using System;
using CrazyGames24;
using UnityEngine;
using UnityEngine.UI;

public class BeatInterface : MonoBehaviour
{
    [SerializeField] private Image beatImg;
    [SerializeField] private GameObject beatPrefab;
    public Transform beatInitialTransform;
    public Transform beatFinalTransform;

    [SerializeField] private Image[] beatFeedback;

    Prebeat currentBeat;

    private void Start()
    {
        GameManager.Instance.player.OnAttachFish.AddListener(SetBeatdetector);
        GameManager.Instance.player.OnDetachFish.AddListener(ReleaseBeatdetector);
    }

    private void SetBeatdetector()
    {
        GameManager.Instance.player.currentFish.beatDetector.BeatOnTime += OnBeat;

        foreach (var b in beatFeedback) b.gameObject.SetActive(false);
    }

    private void ReleaseBeatdetector(Fish currentFish)
    {
        currentFish.beatDetector.BeatOnTime -= OnBeat;
    }

    private void OnBeat(Prebeat prebeat)
    {
        foreach (var b in beatFeedback) b.gameObject.SetActive(false);

        beatFeedback[(int)prebeat.beatStatus].gameObject.SetActive(true);

        GameManager.Instance.player.currentFish.SetTriggerColor((int)prebeat.beatStatus);
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
        GameObject bGO = Instantiate(beatPrefab, GameManager.Instance.player.currentFish.beatDetector.transform.parent);
        bGO.transform.localPosition = GameManager.Instance.player.currentFish.beatInitialTransform.transform.localPosition;

        // bGO.GetComponent<Image>().color = beatImg.color;
        bGO.GetComponent<Prebeat>().Init(GameManager.Instance.player.currentFish.beatFinalTransform.transform.localPosition,
                                            GameManager.Instance.player.currentFish.beatDetector.transform.localPosition);

        currentBeat = bGO.GetComponent<Prebeat>();
        foreach (var b in beatFeedback) b.gameObject.SetActive(false);
    }
}
