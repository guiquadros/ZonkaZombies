using UnityEngine;
using ZonkaZombies.Characters.Enemy.EnemyIA;
using ZonkaZombies.Input;

namespace ZonkaZombies.Characters.Player.Weapon
{
    //IWeaponHandler
    //WeaponHandler
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField]
        private WeaponDetails _weaponDetails;

        [SerializeField]
        private Light _faceLight;

        [SerializeField]
        private Transform _aimStartTransform;

        [SerializeField]
        private ParticleSystem _gunParticles; // Reference to the particle system.

        [SerializeField]
        private LineRenderer _gunLine; // Reference to the line renderer.

        [SerializeField]
        private Light _gunLight; // Reference to the light component.

        public Transform[] IkTransforms = new Transform[2];

        private InputReader _inputReader;

        private float _timer; // A timer to determine when to fire.
        private Ray _shootRay; // A ray from the gun end forwards.
        private RaycastHit _shootHit; // A raycast hit to get information about what was hit.

        private float _effectsDisplayTime = 0.2f; // The proportion of the timeBetweenBullets that the effects will display for.

        private bool _canFire;

        private void Start()
        {
            _timer = _weaponDetails.TimeBetweenBullets;
        }

        public void Initialize(InputReader inputReader)
        {
            _inputReader = inputReader;
        }

        private void LateUpdate()
        {
            // Add the time since Update was last called to the timer.
            _timer += Time.deltaTime;

            _canFire = _inputReader.AnyTrigger() && _timer >= _weaponDetails.TimeBetweenBullets;

            // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
            if (_timer >= _weaponDetails.TimeBetweenBullets * _effectsDisplayTime)
            {
                // ... disable the effects.
                DisableEffects();
            }
        }
        
        private void DisableEffects()
        {
            // Disable the line renderer and the light.
            _gunLine.enabled = false;
            _faceLight.enabled = false;
            _gunLight.enabled = false;
        }
        
        public bool TryToUse()
        {
            if (!_canFire)
            {
                return false;
            }

            // Reset the timer.
            _timer = 0f;

            // Enable the lights.
            _gunLight.enabled = true;
            _faceLight.enabled = true;

            // Stop the particles from playing if they were, then start the particles.
            _gunParticles.Stop();
            _gunParticles.Play();

            // Enable the line renderer and set it's first position to be the end of the gun.
            _gunLine.enabled = true;
            _gunLine.SetPosition(0, _aimStartTransform.position);

            // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            _shootRay.origin = _aimStartTransform.position;
            _shootRay.direction = _aimStartTransform.forward;

            Debug.DrawLine(_shootRay.origin, _shootRay.origin + _shootRay.direction * _weaponDetails.Range, Color.magenta);

            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (Physics.Raycast(_shootRay, out _shootHit, _weaponDetails.Range, Physics.AllLayers))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                GenericEnemy genericEnemy = _shootHit.collider.GetComponent<GenericEnemy>();

                // If the EnemyBehavior component exist...
                if (genericEnemy != null)
                {
                    // ... the enemy should take damage.
                    //TODO get damage from current gun
                    genericEnemy.Damage(_weaponDetails.ShotHitPoints);
                }
                
                // Set the second position of the line renderer to the point the raycast hit.
                _gunLine.SetPosition(1, _shootHit.point);
                //transform.parent.LookAt(_shootHit.point);
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                _gunLine.SetPosition(1, _shootRay.origin + _shootRay.direction * _weaponDetails.Range);
            }

            return true;
        }
    }
}