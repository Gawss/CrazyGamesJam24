using System;
using UnityEngine;

namespace CrazyGames24
{
    public class SFXGameplay : MonoBehaviour
    {
        AudioSource gameplayAudioSource;
        [SerializeField] private AudioClip[] audioClips;

        private void Start()
        {
            GameManager.Instance.player.OnLifePointsChanged += PlayOnHookLost;
            GameManager.Instance.player.OnSongCollected += PlayOnSongCollected;

            gameplayAudioSource = GetComponent<AudioSource>();
        }

        private void OnDestroy()
        {
            GameManager.Instance.player.OnLifePointsChanged -= PlayOnHookLost;
            GameManager.Instance.player.OnSongCollected -= PlayOnSongCollected;
        }


        private void PlayOnSongCollected()
        {
            PlaySongByIndex(0);
        }

        private void PlayOnHookLost()
        {
            PlaySongByIndex(1);
        }

        public void PlaySongByIndex(int index)
        {
            gameplayAudioSource.PlayOneShot(audioClips[index]);
        }
    }
}