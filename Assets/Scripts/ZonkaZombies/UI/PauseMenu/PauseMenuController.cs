using UnityEngine;
using ZonkaZombies.Util;

namespace ZonkaZombies.UI.PauseMenu
{
    public class PauseMenuController : SingletonMonoBehaviour<PauseMenuController>
    {
        private Animator _animator;

        private bool _pauseState;
        
        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        
        public void Pause()
        {
            _pauseState = !_pauseState;
            Time.timeScale = _pauseState ? 0 : 1;
            _animator.SetBool("pause", _pauseState);
        }
    }
}