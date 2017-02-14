using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TimerController : MonoBehaviour
    {
        [SerializeField]
        private Text timer;

        private int minutes = 10, seconds = 0;

        private void Start()
        {
            StartTimer();
        }

        private void OnDestroy ()
        {
            CancelInvoke("UpdateTimer");
        }

        public void StartTimer()
        {
            minutes = seconds = 0;
            //Executa o método UpdateTimer a cada 1 segundo
            InvokeRepeating("UpdateTimer", 0, 1);
        }

        private void UpdateTimer()
        {
            seconds--;
            if (seconds == -1)
            {
                seconds = 59;
                minutes--;
                if (minutes == -1)
                {
                    CancelInvoke();
                    return;
                }
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            timer.text = 
                string.Format("{0}:{1}", 
                    minutes > 9 ? minutes.ToString() : string.Format("0{0}", minutes),
                    seconds > 9 ? seconds.ToString() : string.Format("0{0}", seconds));
        }
    }
}
