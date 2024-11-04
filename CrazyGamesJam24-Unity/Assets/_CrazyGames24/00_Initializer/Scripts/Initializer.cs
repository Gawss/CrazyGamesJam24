using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CrazyGames24
{
    public class Initializer : MonoBehaviour
    {
        private bool isInitialized = false;
        private bool clearPreviousScene = false;
        private SceneInstance prevScene;

        AsyncOperationHandle<IResourceLocator> addressablesLoadOperation;

        AsyncOperationHandle<SceneInstance> sceneLoaderOperation;

        public Action OnGameSceneLoaded;

        [SerializeField] private string[] sceneNames;

        private void OnAwake()
        {
#if DEVELOPMENT_BUILD
            Debug.unityLogger.logEnabled=true;
#else
            Debug.unityLogger.logEnabled = false;
#endif
        }

        private void Start()
        {
            ResourceManager.ExceptionHandler = CustomExceptionHandler;

            addressablesLoadOperation = Addressables.InitializeAsync();

            addressablesLoadOperation.Completed += OnInitialized;

            StartCoroutine(DisplayProgress(addressablesLoadOperation));

        }

        void CustomExceptionHandler(AsyncOperationHandle handle, Exception exception)
        {
            Debug.Log("CUSTOM EXCEPTION HANDLER");
            Debug.LogWarning(exception);

            // ErrorCanvas.Instance.DisplayError(true);

            // if (exception.GetType() != typeof(InvalidKeyException))
            //     Addressables.LogException(handle, exception);
        }

        private void OnInitialized(AsyncOperationHandle<IResourceLocator> handle)
        {
            addressablesLoadOperation.Completed -= OnInitialized;

            isInitialized = true;
            prevScene = new SceneInstance();

            LoadAddressableScene(sceneNames[0]);
        }

        public void LoadSceneByIndex(int index)
        {
            LoadAddressableScene(sceneNames[index]);
        }

        public void LoadAddressableScene(string addressableKey)
        {
            Debug.Log("Load Addressable Scene: " + addressableKey);
            if (!isInitialized) return;

            if (clearPreviousScene && SceneManager.loadedSceneCount > 1)
            {
                Debug.Log("Clearing previous scene, " + prevScene.Scene.name);

                AsyncOperationHandle unloadSceneOperation = Addressables.UnloadSceneAsync(prevScene);

                unloadSceneOperation.Completed += (asyncHandle) =>
                {
                    if (asyncHandle.Status == AsyncOperationStatus.Failed)
                    {
                        Debug.Log("Operation Exception");
                        return;
                    }

                    clearPreviousScene = false;
                    prevScene = new SceneInstance();

                    Debug.Log("Previous Scene cleared");

                    StartCoroutine(UnloadUnusedAssets(() =>
                    {
                        Debug.Log("Unused Assets Unloaded");
                        DownloadDependenciesAndLoadScene(addressableKey);
                    }));
                };
            }
            else
            {
                DownloadDependenciesAndLoadScene(addressableKey);
            }


        }

        public void LoadSceneAdditiveByIndex(int index)
        {
            DownloadDependenciesAndLoadScene(sceneNames[index]);
        }

        private void DownloadDependenciesAndLoadScene(string addressableKey)
        {
            Debug.Log("Downloading Dependencies");
            AsyncOperationHandle downloadDependenciesOperation = Addressables.DownloadDependenciesAsync(addressableKey, true);


            downloadDependenciesOperation.Completed += (handle) =>
            {
                Debug.Log("Download Dependencies Completed");
                Debug.Log("Loading AddressableKey -> " + addressableKey);
                // Addressables.Release(downloadDependenciesOperation);

                sceneLoaderOperation = Addressables.LoadSceneAsync(addressableKey, LoadSceneMode.Additive);

                sceneLoaderOperation.Completed += (asyncHandle) =>
                {
                    Debug.Log("Scene Loader Operation Completed");

                    if (asyncHandle.OperationException != null)
                    {
                        Debug.Log("Operation Exception Not null");
                        if (asyncHandle.OperationException.GetType() != typeof(InvalidKeyException))
                        {
                            Debug.Log($"Addressables Scene couldn't be loaded -> {addressableKey}");
                            return;
                        }
                    }


                    clearPreviousScene = true;
                    prevScene = asyncHandle.Result;

                    Debug.Log($"Loading Scene {addressableKey}");

                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(addressableKey));

                    OnGameSceneLoaded?.Invoke();
                };

                // StartCoroutine(DisplayProgress(sceneLoaderOperation));

            };

            StartCoroutine(DisplayProgress(downloadDependenciesOperation));
        }

        private IEnumerator UnloadUnusedAssets(Action OnComplete)
        {
            yield return Resources.UnloadUnusedAssets();

            OnComplete?.Invoke();
        }

        private IEnumerator DisplayProgress(AsyncOperationHandle operation)
        {

            float downloadTimer = 0;

            while (!operation.IsDone)
            {
                downloadTimer += Time.deltaTime;

                yield return 0f;
            }

            Debug.Log(operation.IsValid());

            if (!operation.IsValid()) yield break;

            Debug.Log("Addressables Operation Completed");
        }
    }
}