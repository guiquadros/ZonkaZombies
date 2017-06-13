using UnityEngine;
using ZonkaZombies.Characters;
using ZonkaZombies.Characters.Player.Behaviors;

namespace ZonkaZombies.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [Header("HealthBar")] public RectTransform HealthBar;
        public RectTransform HealthBarEmpty;
        public RectTransform HealthBarFull;
        public RectTransform HealthBarBlink;

        [Header("Blink")] public bool AutoBlink;
        public float BlinkThreshold = 0.25f;
        public Animator BlinkAnimator;

        private void Awake()
        {
            //Force this object to start disabled
            gameObject.SetActive(false);
        }

        public void Initialize(Player player) 
        {
            //Subscribe to player callback
            player.Health.AddListener(OnHealthChangedCallback);

            //Properly start the health bar value
            OnHealthChangedCallback(player.Health.Current, 0, player);

            gameObject.SetActive(true);
        }

        public void Uninitialize(Player player)
        {
            //Remove subscribed callbacks
            player.Health.RemoveListener(OnHealthChangedCallback);

            gameObject.SetActive(false);
        }

#region CALLBACKS - PLAYER

        private void OnHealthChangedCallback(int currentHealth, int damageTaken, Character player)
        {
            float fillAmount = (float) currentHealth / (float) player.Health.Maximum;
            UpdateHealthBar(fillAmount);
        }
        
#endregion

#region HELPER METHODS
        
        private void SetHealthBarBlinkState(bool state)
        {
            BlinkAnimator.SetBool("Blink", state);
        }

        private void UpdateHealthBar(float fillAmount)
        {
            HealthBar.anchoredPosition = Vector2.Lerp(HealthBarEmpty.anchoredPosition, HealthBarFull.anchoredPosition, fillAmount);

            if (AutoBlink)
            {
                SetHealthBarBlinkState(BlinkThreshold >= fillAmount);
            }
        }

        #endregion
    }
}
