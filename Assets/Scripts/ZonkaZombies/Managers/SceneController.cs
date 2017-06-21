using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Util;

// This script exists in the Persistent scene and manages the content
// based scene's loading.  It works on a principle that the
// Persistent scene will be loaded first, then it loads the scenes that
// contain the player and other visual elements when they are needed.
// At the same time it will unload the scenes that are not needed when
// the player leaves them.
namespace ZonkaZombies.Managers
{
    public class SceneController : SingletonMonoBehaviour<SceneController>
    {
        public enum GameState
        {
            Playing, Loading
        }

        public event Action BeforeSceneUnload;          // Event delegate that is called just before a scene is unloaded.
        public event Action AfterSceneLoad;             // Event delegate that is called just after a scene is loaded.
        public event Action<GameSceneType> OnSceneLoading;
        public CanvasGroup faderCanvasGroup;            // The CanvasGroup that controls the Image used for fading to black.

        //TODO: the fade was causing many problems to load the scenes async. Turning it to 0 seems to have solved the problem.
        public float fadeDuration = 0f;                 // How long it should take to fade to and from black.

        private GameState _currentGameState = GameState.Playing;
        private bool isFading;                          // Flag used to determine if the Image is currently fading to or from black.

        [SerializeField]
        public int CurrentSceneIndex = 0;             // The index of the current scene.

        private GameSceneType _currentScene;

        [SerializeField]
        private GameSceneType _previousScene;
        

        private IEnumerator Start ()
        {
            _currentScene = GameScenes.GameScenesOrdered.First();

            // Set the initial alpha to start off with a black screen.
            faderCanvasGroup.alpha = 1f;

            // Start the first scene loading and wait for it to finish.
            yield return StartCoroutine (LoadSceneAndSetActive (_currentScene));

            // Once the scene is finished loading, start fading in.
            StartCoroutine (Fade (0f));
        }

        private int tryReload = 0;

        public void LoadNextScene()
        {
            if (_currentGameState == GameState.Loading)
            {
                tryReload++;

                if (tryReload == 2)
                {
                    tryReload = 0;

                    Debug.Log("Trying to reload the scene.");
                    StopAllCoroutines();
                    FadeAndLoadScene(_currentScene);
                }
                else
                {
                    Debug.Log("Scene cannot be loaded because you are already loading a scene");
                }
                return;
            }

            Debug.LogFormat("LoadNextScene() - start: _currentSceneName = {0}", _currentScene);
            CurrentSceneIndex++;

            GameSceneType[] scenes = GameScenes.GameScenesOrdered;

            if (CurrentSceneIndex >= scenes.Length)
            {
                CurrentSceneIndex = 0;
            }

            _previousScene = _currentScene;
            _currentScene = scenes[CurrentSceneIndex];

            Debug.LogFormat("LoadNextScene() - end: _currentSceneName = {0}", _currentScene);
            _currentGameState = GameState.Loading;
            FadeAndLoadScene(_currentScene);
        }

        public void FadeAndLoadScene(GameSceneType gameScene)
        {
            Debug.Log("FadeAndSwitchScenes()");
            EntityManager.Instance.Enemies.Clear();

            // If a fade isn't happening then start fading and switching scenes.
            //if (!isFading)
            //{
                StartCoroutine(FadeAndSwitchScenes(gameScene));
            //}
        }

        // This is the coroutine where the 'building blocks' of the script are put together.
        private IEnumerator FadeAndSwitchScenes (GameSceneType gameScene)
        {
            // Start fading to black and wait for it to finish before continuing.
            yield return StartCoroutine (Fade (1f));

            // If this event has any subscribers, call it.
            if (BeforeSceneUnload != null)
                BeforeSceneUnload ();

            Debug.Log("Current active scene: " + SceneManager.GetActiveScene().name);

            // Start loading the given scene and wait for it to finish.
            yield return StartCoroutine (LoadSceneAndSetActive (gameScene));
            
            // Start fading back in and wait for it to finish before exiting the function.
            yield return StartCoroutine (Fade (0f));
        }


        private IEnumerator LoadSceneAndSetActive (GameSceneType gameScene)
        {
            //Debug.Log("LoadSceneAndSetActive()");

            if (OnSceneLoading != null)
            {
                OnSceneLoading(gameScene);
            }

            // Allow the given scene to load over several frames and add it to the already loaded scenes (just the Persistent scene at this point).
            yield return SceneManager.LoadSceneAsync (gameScene.SceneName, LoadSceneMode.Additive);
            
            if (gameScene.UnloadAllOtherScenes)
            {
                // Unload the current active scene.
                //yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                var allScenes = SceneManager.GetAllScenes();

                foreach (var scene in allScenes)
                {
                    if (scene.name != SceneConstants.PERSISTENT && scene.name != gameScene.SceneName)
                    {
                        yield return SceneManager.UnloadSceneAsync(scene.name);
                    }
                }
            }

            // Find the scene that was most recently loaded (the one at the last index of the loaded scenes).
            Scene newlyLoadedScene = SceneManager.GetSceneAt (SceneManager.sceneCount - 1);

            // Set the newly loaded scene as the active scene (this marks it as the one to be unloaded next).
            SceneManager.SetActiveScene (newlyLoadedScene);

            _currentGameState = GameState.Playing;

            // If this event has any subscribers, call it.
            if (AfterSceneLoad != null)
            {
                AfterSceneLoad();
            }
        }


        private IEnumerator Fade (float finalAlpha)
        {
            // Set the fading flag to true so the FadeAndSwitchScenes coroutine won't be called again.
            isFading = true;

            // Make sure the CanvasGroup blocks raycasts into the scene so no more input can be accepted.
            faderCanvasGroup.blocksRaycasts = true;

            // Calculate how fast the CanvasGroup should fade based on it's current alpha, it's final alpha and how long it has to change between the two.
            float fadeSpeed = Mathf.Abs (faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

            // While the CanvasGroup hasn't reached the final alpha yet...
            while (!Mathf.Approximately (faderCanvasGroup.alpha, finalAlpha))
            {
                // ... move the alpha towards it's target alpha.
                faderCanvasGroup.alpha = Mathf.MoveTowards (faderCanvasGroup.alpha, finalAlpha,
                    fadeSpeed * Time.deltaTime);

                // Wait for a frame then continue.
                yield return null;
            }

            // Set the flag to false since the fade has finished.
            isFading = false;

            // Stop the CanvasGroup from blocking raycasts so input is no longer ignored.
            faderCanvasGroup.blocksRaycasts = false;
        }
    }
}