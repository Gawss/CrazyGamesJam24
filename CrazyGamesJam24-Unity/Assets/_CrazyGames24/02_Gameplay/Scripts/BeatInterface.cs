using System;
using CrazyGames24;
using UnityEngine;
using UnityEngine.UI;

public class BeatInterface : MonoBehaviour
{
    public BeatDetector beatDetector;

    [SerializeField] private Image beatImg;

    private void OnEnable()
    {
        beatDetector.OnDrumDetected += OnDrumDetected;
        beatDetector.OnHitDetected += OnHitDetected;
        beatDetector.OnTransientDetected += OnTransientDetected;
        beatDetector.OnNothing += OnNothing;
    }

    private void OnNothing()
    {
        beatImg.color = Color.black;
    }

    private void OnDisable()
    {
        beatDetector.OnDrumDetected -= OnDrumDetected;
        beatDetector.OnHitDetected -= OnHitDetected;
        beatDetector.OnTransientDetected -= OnTransientDetected;
        beatDetector.OnNothing -= OnNothing;
    }

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
}
