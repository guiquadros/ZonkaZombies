using UnityEngine;
// ReSharper disable InconsistentNaming

namespace ZonkaZombies.Util
{
    public static class LayerConstants
    {
        public static readonly int ENEMY_LAYER = LayerMask.NameToLayer("Enemy");
        public static readonly int PLAYER_CHARACTER_LAYER = LayerMask.NameToLayer("PlayerCharacter");
    }

    public static class TagConstants
    {
        public const string PLAYER_DAMAGER = "PlayerDamager";
    }

    public static class SceneConstants
    {
        public const string GAME_OVER_SCENE_NAME = "GameOver";
        public const string MANY_ENEMIES_VS_CHARACTER = "ManyEnemiesVsCharacter";
    }
}
