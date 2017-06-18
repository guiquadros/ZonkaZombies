using System;
using UnityEngine;
using ZonkaZombies.Messaging;
using ZonkaZombies.Messaging.Messages.UI;

namespace ZonkaZombies.Controllers
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        [Serializable]
        public class ViewConfigutation
        {
            public BaseView Previous;
            public BaseView Next;
            public Transform Transform;
        }

        public ViewConfigutation Configuration;

        public virtual void OnShow()
        {
            gameObject.SetActive(true);
        }
        
        public virtual void OnHide()
        {
            gameObject.SetActive(false);
        }

        public void CallView()
        {
            var message = new MoveCameraMessage()
            {
                Position = Configuration.Transform.position,
                Rotation = Configuration.Transform.rotation
            };
            MessageRouter.SendMessage(message);

            OnShow();
        }

        private void TryCallView(BaseView view)
        {
            if (view != null)
            {
                view.CallView();
                OnHide();
            }
        }

        protected void CallPreviousView()
        {
            TryCallView(Configuration.Previous);
        }

        protected void CallNextView()
        {
            TryCallView(Configuration.Next);
        }
    }
}