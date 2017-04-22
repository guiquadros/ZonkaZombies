using UnityEngine;
using ZonkaZombies.Prototype.Characters.Enemy;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Characters.PlayerCharacter
{
    public class PlayerShooting : MonoBehaviour
    {
        public int ShotHitPoints
        {
            //the shot gives a random damage
            //TODO: We can have a specific enemy animations based in the intensity of the damage. For instance, a damage = 1 can be a shot in the chest and a damage = 3 can be a shot in the head, then we avoid localized damage logic/physics.
            get { return Random.Range(1, 3); }
        }

        [SerializeField]
        private float _timeBetweenBullets = 0.15f; // The time between each shot.

        [SerializeField]
        private float _range = 100f; // The distance the gun can fire.

        [SerializeField]
        private Light _faceLight;

        [SerializeField]
        private Player _abstractPlayerCharacterBehavior;

        private float _timer; // A timer to determine when to fire.
        private Ray _shootRay = new Ray(); // A ray from the gun end forwards.
        private RaycastHit _shootHit; // A raycast hit to get information about what was hit.
        private int _enemyLayerMask; // A layer mask so the raycast only hits enemies.
        private ParticleSystem _gunParticles; // Reference to the particle system.
        private LineRenderer _gunLine; // Reference to the line renderer.
        private AudioSource _gunAudio; // Reference to the audio source.
        private Light _gunLight; // Reference to the light component.

        private float _effectsDisplayTime = 0.2f; // The proportion of the timeBetweenBullets that the effects will display for.


        private void Awake()
        {
            // Create a layer mask for the Shootable layer.
            _enemyLayerMask = LayerMask.GetMask(LayerConstants.ENEMY_LAYER_NAME);

            // Set up the references.
            _gunParticles = GetComponent<ParticleSystem>();
            _gunLine = GetComponent<LineRenderer>();
            _gunAudio = GetComponent<AudioSource>();
            _gunLight = GetComponent<Light>();
        }


        private void Update()
        {
            // Add the time since Update was last called to the timer.
            _timer += Time.deltaTime;

            // If the Fire1 button is being press and it's time to fire...
            if (_abstractPlayerCharacterBehavior.InputReader.RightTrigger() && _timer >= _timeBetweenBullets &&
                Time.timeScale != 0)
            {
                // ... shoot the gun.
                Shoot();
            }

            // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
            if (_timer >= _timeBetweenBullets * _effectsDisplayTime)
            {
                // ... disable the effects.
                DisableEffects();
            }
        }


        public void DisableEffects()
        {
            // Disable the line renderer and the light.
            _gunLine.enabled = false;
            _faceLight.enabled = false;
            _gunLight.enabled = false;
        }


        private void Shoot()
        {
            // Reset the timer.
            _timer = 0f;

            // Play the gun shot audioclip.
            _gunAudio.Play();

            // Enable the lights.
            _gunLight.enabled = true;
            _faceLight.enabled = true;

            // Stop the particles from playing if they were, then start the particles.
            _gunParticles.Stop();
            _gunParticles.Play();

            // Enable the line renderer and set it's first position to be the end of the gun.
            _gunLine.enabled = true;
            _gunLine.SetPosition(0, transform.position);

            // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            _shootRay.origin = transform.position;
            _shootRay.direction = transform.forward;

            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (Physics.Raycast(_shootRay, out _shootHit, _range, _enemyLayerMask))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                Enemy.Enemy enemy = _shootHit.collider.GetComponent<Enemy.Enemy>();

                // If the EnemyBehavior component exist...
                if (enemy != null)
                {
                    // ... the enemy should take damage.
                    enemy.Damage(ShotHitPoints);
                }

                // Set the second position of the line renderer to the point the raycast hit.
                _gunLine.SetPosition(1, _shootHit.point);
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                _gunLine.SetPosition(1, _shootRay.origin + _shootRay.direction * _range);
            }
        }
    }
}