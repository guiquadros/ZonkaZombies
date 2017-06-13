using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZonkaZombies.Scenery.Interaction
{
    public class HealthPackInteractable : CollectableInteractable
    {
        [SerializeField, Range(1, 50)]
        private int _lifePoints;

        private bool _isUsed;

        private bool _animateScale;

        public override void Collect()
        {
            if (_isUsed)
                return;

            _isUsed = true;

            // Heal the player
            PlayerInteracting.Health.Add(_lifePoints);
            _animateScale = true;
            StopGlowing();
        }

        private void LateUpdate()
        {
            if (!_animateScale)
                return;

            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime * 8f);

            if (PlayerInteracting != null)
            {
                Vector3 stepPosition = PlayerInteracting.transform.position;
                stepPosition.y = transform.position.y;
                transform.position = Vector3.MoveTowards(transform.position, stepPosition,
                    Time.deltaTime * 20f);
            }

            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);
            }
        }
    }
}