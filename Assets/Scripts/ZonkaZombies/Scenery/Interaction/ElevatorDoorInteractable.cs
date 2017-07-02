using System.Collections.Generic;
using UnityEngine;
using ZonkaZombies.Managers;
using ZonkaZombies.Spawn;
using ZonkaZombies.UI;
using ZonkaZombies.Util;
using ZonkaZombies.Util.Commands;

namespace ZonkaZombies.Scenery.Interaction
{
    public class ElevatorDoorInteractable : DoorInteractable
    {
        private SpawnManager[] _spawnManagers;

        [SerializeField]
        private Animator _elevatorAnimator;

        [SerializeField]
        private List<BaseCommand> _commands;

        [SerializeField]
        private AudioClip _buttonClickClip;
        [SerializeField]
        private AudioClip _elevatorClip;
        [SerializeField]
        private AudioClip _openingDoorClip;

        [SerializeField]
        private AudioSource _elevatorAudioSource;

        [SerializeField]
        private float _elevatorTime = 60f;

        private bool _elevatorCalled = false;

        private float _time;

        [SerializeField]
        private TextMesh _timeTextMesh;

        private bool _animCalled;

        protected override void Start()
        {
            var transparentRed = Color.red;
            transparentRed.a = 0f;

            _renderer.material.color = transparentRed;
            _navMeshObstacle.enabled = true;
            _collider.enabled = true;
            _time = _elevatorTime;

            _spawnManagers = FindObjectsOfType<SpawnManager>();
        }

        private void Update()
        {
            if (_time <= 2f && !_animCalled)
            {
                _animCalled = true;
                _elevatorAnimator.SetTrigger(ElevatorAnimatorParameters.CALL_ELEVATOR_ID);
            }

            if (_elevatorCalled && _time > 0f)
            {
                _time -= Time.deltaTime;
                _time = _time <= 0f ? 0f : _time;
            }

            string timeStr = _time.ToString("##");
            _timeTextMesh.text = string.IsNullOrEmpty(timeStr) ? "0" : timeStr;

            GameUIManager.Instance.UpdateElevatorNumber(_timeTextMesh.text);
        }

        public override void OnBegin(IInteractor interactor)
        {
            if (!_elevatorCalled)
            {
                GameUIManager.Instance.MarkPressElevatorButtonTextAsCompleted();
                _elevatorCalled = true;
                _commands.ForEach(c => c.Execute());
                AudioManager.Instance.Play(_buttonClickClip);
                Invoke("UpdateDoorState", _elevatorTime);
                StopGlowing();

                _elevatorAudioSource.clip = _elevatorClip;
                _elevatorAudioSource.loop = true;
                _elevatorAudioSource.volume = .6f;
                _elevatorAudioSource.Play();
            }
        }

        public override void OnFinish(IInteractor interactor)
        {
            // Not necessary...
        }

        protected override void UpdateDoorState()
        {
            if (_renderer == null || _collider == null)
            {
                return;
            }

            _navMeshObstacle.enabled = false;
            _collider.enabled = false;

            AudioManager.Instance.Play(_openingDoorClip);
            _elevatorAudioSource.Stop();

            StopAllSpawns();
        }

        private void StopAllSpawns()
        {
            if (_spawnManagers != null && _spawnManagers.Length > 0)
            {
                foreach (var spawnManager in _spawnManagers)
                {
                    spawnManager.StopSpawn();
                }
            }
        }
    }
}