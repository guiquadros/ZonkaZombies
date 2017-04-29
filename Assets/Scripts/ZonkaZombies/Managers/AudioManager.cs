using UnityEngine;
using ZonkaZombies.Multiplayer;

namespace ZonkaZombies.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get
            {
                // ReSharper disable once InvertIf
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AudioManager>();

                    if (_instance == null)
                    {
                        GameObject go = new GameObject("AudioManager");
                        _instance = go.AddComponent<AudioManager>();
                        Debug.LogWarning("No 'AudioManager' component was found in the scene, so I've created one for you!");
                    }

                    _instance.Initialize();
                }

                return _instance;
            }
        }

        private SplitscreenHandler _splitscreenHandler;
        private AudioSource _audioSource;

        private void Initialize()
        {
            _audioSource = GetComponent<AudioSource>();
            _splitscreenHandler = FindObjectOfType<SplitscreenHandler>();
        }

        private void LateUpdate()
        {
            if (_splitscreenHandler == null)
            {
                return;
            }

            // Moves this AudioSource to the middle position between the two splited cameras
            transform.position = _splitscreenHandler.GetCentralPosition();
        }

        public void PlayEffect(AudioClip clip, float volume = 1.0f)
        {
            if (clip == null)
            {
                return;
            }
            _audioSource.PlayOneShot(clip, Mathf.Clamp01(volume));
        }
    }
}