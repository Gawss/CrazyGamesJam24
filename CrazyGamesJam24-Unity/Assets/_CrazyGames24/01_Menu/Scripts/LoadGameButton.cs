using System;
using UnityEngine;
using UnityEngine.UI;

namespace CrazyGames24
{
    public class LoadGameButton : MonoBehaviour
    {
        private Button targetButton;
        [SerializeField] private int targetScene;

        Initializer initializer;

        private void OnEnable()
        {
            targetButton = GetComponent<Button>();
            targetButton.onClick.AddListener(LoadGame);

            initializer = FindAnyObjectByType<Initializer>();
        }

        private void LoadGame()
        {
            targetButton.onClick.RemoveListener(LoadGame);
            initializer.LoadSceneByIndex(targetScene);
        }

        private void OnDisable()
        {
            targetButton.onClick.RemoveAllListeners();
        }
    }
}