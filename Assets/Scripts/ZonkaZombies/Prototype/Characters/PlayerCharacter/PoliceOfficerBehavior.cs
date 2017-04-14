using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Input;
using ZonkaZombies.Prototype.Characters.Enemy;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Characters.PlayerCharacter
{
    public class PoliceOfficerBehavior : PlayerCharacterBehavior
    {
        [SerializeField]
        private GameObject _enemies;

        protected override void Awake()
        {
            base.Awake();

            Type = PlayerCharacterType.PoliceOfficer;
        }

        protected override void Update()
        {
            base.Update();

            //TODO: find a better way to do this in terms of performance. The PoliceOfficerBehavior class is not the best place to put the win condition (we should have a GameManager class).
            if (_enemies != null)
            {
                EnemyBehavior[] childrenComponents = _enemies.GetComponentsInChildren<EnemyBehavior>();

                if (childrenComponents.Length == 0)
                {
                    SceneManager.LoadScene(SceneConstants.PLAYER_WIN_SCENE_NAME);
                }
            }
        }
    }
}