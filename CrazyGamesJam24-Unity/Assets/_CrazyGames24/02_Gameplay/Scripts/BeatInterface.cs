using System;
using CrazyGames24;
using UnityEngine;
using UnityEngine.UI;

public class BeatInterface : MonoBehaviour
{
    public BeatDetector beatDetector;

    [SerializeField] private Image beatImg;
    [SerializeField] private GameObject beatPrefab;
    [SerializeField] private Transform beatInitialTransform;

    // private void OnEnable()
    // {
    //     beatDetector.OnDrumDetected += OnDrumDetected;
    //     beatDetector.OnHitDetected += OnHitDetected;
    //     beatDetector.OnTransientDetected += OnTransientDetected;
    //     beatDetector.OnNothing += OnNothing;
    // }

    private void OnNothing()
    {
        beatImg.color = Color.black;
    }

    // private void OnDisable()
    // {
    //     beatDetector.OnDrumDetected -= OnDrumDetected;
    //     beatDetector.OnHitDetected -= OnHitDetected;
    //     beatDetector.OnTransientDetected -= OnTransientDetected;
    //     beatDetector.OnNothing -= OnNothing;
    // }

    private void OnTransientDetected()
    {
        beatImg.color = Color.magenta;
    }

    private void OnHitDetected()
    {
        beatImg.color = Color.red;
    }

    private void OnDrumDetected()
    {
        beatImg.color = Color.yellow;
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
        bGO.transform.SetPositionAndRotation(beatInitialTransform.position, beatInitialTransform.rotation);

        bGO.GetComponent<Image>().color = beatImg.color;
        bGO.GetComponent<Prebeat>().Init(beatImg.transform.position);
    }
}
