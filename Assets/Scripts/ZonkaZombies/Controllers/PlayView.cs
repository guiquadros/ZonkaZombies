using UnityEngine;
using ZonkaZombies.Input;
using UnityInput = UnityEngine.Input;

namespace ZonkaZombies.Controllers
{
    public class PlayView : BaseView
    {
        private InputReader _inputReader;

        public override void OnShow()
        {
            base.OnShow();

            _inputReader = InputFactory.Create(InputType.Controller1);
        }

        public override void OnHide()
        {
            base.OnHide();

            _inputReader = null;
        }

        private void Update()
        {
            if (UnityInput.GetKeyDown(KeyCode.Escape))
            {
                CallPreviousView();
            }
        }
    }
}