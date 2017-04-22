using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using ZonkaZombies.Prototype.Managers;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Characters.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Character
    {
        public MeshFilter MeshFilter;
        public Mesh Mesh;

        [SerializeField]
        private Transform _target;

        [SerializeField]
        private Transform _playerCharacterTransform;

        [SerializeField]
        private bool _useFieldOfView = false, _agentStopped = false;

        protected NavMeshAgent Agent;

        [SerializeField]
        private float _minPlayerDetectDistance = 5.0f, _fieldOfVisionTimeout = 5f;

        [SerializeField]
        private float _fieldOfViewAngle = 68.0f; // in degrees (I use 68, this gives the enemy a vision of 136 degrees)

        private float _timeWithoutSeeingThePlayer;

        //TODO Move this to a ScriptableObject so It can be easily accessed by each enemy on scene
        [SerializeField]
        private AudioClip _damagedAudioClip;

        protected virtual void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            MeshFilter = GetComponent<MeshFilter>();
            Mesh = new Mesh();
        }

        protected virtual void Update()
        {
            if (_target == null)
                return;

            if (CanSeePlayerCharacter())
            {
                //Debug.Log("Can see the player");
                Agent.SetDestination(_target.position);
                _timeWithoutSeeingThePlayer = 0f;

                if (_agentStopped)
                {
                    Agent.Resume();
                }
            }
            else
            {
                //Debug.Log("Can NOT see the player");
                _timeWithoutSeeingThePlayer += Time.deltaTime;
            }
            
            //stops the pursuit after some time without see the player
            if (_timeWithoutSeeingThePlayer >= _fieldOfVisionTimeout)
            {
                _timeWithoutSeeingThePlayer = 0f;
                Agent.Stop();

                _agentStopped = true;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerConstants.PLAYER_CHARACTER_LAYER)
            {
                PlayerCharacter.Player abstractPlayerCharacter = other.gameObject.GetComponent<PlayerCharacter.Player>();
#if UNITY_EDITOR
                if (abstractPlayerCharacter.CanReceiveDamage)
#endif
                    abstractPlayerCharacter.Damage(HitPoints, () => SceneManager.LoadScene(SceneConstants.GAME_OVER_SCENE_NAME));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //The enemy was punched by the player
            if (other.CompareTag(TagConstants.PLAYER_DAMAGER))
            {
                var playerCharacter = other.gameObject.GetComponentInParent<PlayerCharacter.Player>();
                this.Damage(playerCharacter.HitPoints, () => Destroy(this.gameObject));
            }
        }

        protected override void OnTakeDamage(int damage)
        {
            AudioManager.Instance.PlayEffect(_damagedAudioClip);
        }

        private bool CanSeePlayerCharacter()
        {
            if (!_useFieldOfView) return true;

            RaycastHit hit;
            Vector3 rayDirection = _playerCharacterTransform.position - transform.position;
            float distanceToPlayer = Vector3.Distance(transform.position, _playerCharacterTransform.position);

            //raycast to the player direction
            bool raycastObj = Physics.Raycast(transform.position, rayDirection, out hit);

            //verify if the object hit is a player character
            bool playerHit = hit.transform.CompareTag(TagConstants.PLAYER);

            //verify if the distance to the player matches the min distance
            bool matchedMinDistance = distanceToPlayer <= _minPlayerDetectDistance;

            float angle = Vector3.Angle(rayDirection, transform.forward);

            //verify the player is in the angle range of the field of vision of the enemy
            bool matchedFieldOfVisionAngle = angle < _fieldOfViewAngle;
            
            return raycastObj && playerHit && (matchedMinDistance || matchedFieldOfVisionAngle);
        }

        void OnDrawGizmos()
        {
            if (MeshFilter == null) return;

            Mesh.Clear();

            Vector3 v0 = this.transform.position;
            Vector3 v1 = this.transform.position - this.transform.right * 3f + this.transform.forward * 5f;
            Vector3 v2 = this.transform.position + this.transform.forward * 5f;
            Vector3 v3 = this.transform.position + this.transform.right * 3f + this.transform.forward * 5f;

            Mesh.vertices = new Vector3[] { v0, v1, v2, v3 };
            Mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

            MeshFilter.mesh = Mesh;
        }
    }
}