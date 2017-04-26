using System.Linq;
using UnityEngine.SceneManagement;
using ZonkaZombies.Input;
using ZonkaZombies.Prototype.Characters;
using ZonkaZombies.Prototype.Scenery.Interaction;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Managers
{
    public enum GameModeType
    {
        Singleplayer, Multiplayer
    }

    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        private InputReader _inputReader;

        protected override void Awake()
        {
            base.Awake();
            _inputReader = InputFactory.Create(InputType.Controller1);
        }

        private EntityManager _entityManager;
        private int toDoMissionsCount;

        public GameModeType GameMode { get; set; }

        private void Start()
        {
            _entityManager = EntityManager.Instance;

            foreach (var enemy in _entityManager.Enemies)
            {
                enemy.OnDead += OnEnemyDead;
            }

            foreach (var player in _entityManager.Players)
            {
                player.OnDead += OnPlayerDead;
            }

            foreach (InteractableBase interactable in FindObjectsOfType<InteractableBase>())
            {
                interactable.OnInteract += OnGetInteractable;
            }
        }

        private void Update()
        {
            if (_inputReader.StartDown())
            {
                //TODO: fix: is entering many times here
                SceneController.Instance.LoadNextScene();
            }
        }

        private void OnEnemyDead(Character character)
        {
            // ReSharper disable once DelegateSubtraction
            character.OnDead -= OnEnemyDead;

            // Don't destroy the enemy, just disable It
            character.gameObject.SetActive(false);

            if (_entityManager.AreAllEnemiesDead())
            {
                SceneManager.LoadScene(SceneConstants.PLAYER_WIN_SCENE_NAME);
            }
        }

        private void OnPlayerDead(Character character)
        {
            SceneManager.LoadScene(SceneConstants.GAME_OVER_SCENE_NAME);
        }

        private void OnGetInteractable(InteractableBase interactable)
        {
            //TODO
        }
    }
}