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

        private AudioSource _audioSource;

        private void Initialize()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Play(AudioClip clip, float volume = 1.0f)
        {
            //TODO Able to change the pitch before playing the sound effect

            if (clip == null)
            {
                return;
            }
            _audioSource.PlayOneShot(clip, Mathf.Clamp01(volume));
        }
    }
}