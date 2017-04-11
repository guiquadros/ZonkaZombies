using UnityEngine;
using UnityEngine.SceneManagement;
using ZonkaZombies.Util;

namespace ZonkaZombies.Prototype.Scenery
{
    public class DeadAbismBehavior : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerConstants.PLAYER_CHARACTER_LAYER)
            {
                SceneManager.LoadScene(SceneConstants.GAME_OVER_SCENE_NAME);
            }
        }
    }
}
