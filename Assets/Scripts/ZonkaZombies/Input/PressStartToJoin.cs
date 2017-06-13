using UnityEngine;
using ZonkaZombies.Characters.Player.Behaviors;
using ZonkaZombies.Managers;

namespace ZonkaZombies.Input
{
    /// <summary>
    /// This class is responsible of instantiate new player characters during the gameplay. Consider a refactoring to allow the selection of new player characters in the future.
    /// </summary>
    public class PressStartToJoin : MonoBehaviour
    {
        [SerializeField]
        private GameObject _playerPrefab;

        [SerializeField]
        private Material _material;

        [SerializeField]
        private SkinnedMeshRenderer _skinnedMeshRenderer;

        private void Update()
        {
            //if _playerPrefab != null (the player 2 was not in game before) and the Start button was pressed in the player 2 controller and  we have only 1 player playing then... 
            if (_playerPrefab != null && PlayerInput.InputReaderController2.Start() && EntityManager.Instance.Players.Count == 1)
            {
                //instantiate the player -- after this Awake() and Start() should happen and do the rest of the job...
                //Vector3.zero, Quaternion.identity - consider the use of player spawn points n the future
                GameObject player2Instance = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);

                //force the input of the player to Controller2
                Player player = player2Instance.GetComponent<Player>();
                player.InputType = InputType.Controller2;

                //force the material
                _skinnedMeshRenderer = player2Instance.GetComponentInChildren<SkinnedMeshRenderer>();
                _skinnedMeshRenderer.material = _material;

                _playerPrefab = null;
            }
        }
    }
}
