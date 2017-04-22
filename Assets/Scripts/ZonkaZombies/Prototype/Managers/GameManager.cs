using UnityEngine.SceneManagement;
using ZonkaZombies.Prototype.Characters;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Managers
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        private EntityManager _entityManager;

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
    }
}