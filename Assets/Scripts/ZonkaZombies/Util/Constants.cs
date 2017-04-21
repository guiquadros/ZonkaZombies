using UnityEngine;
// ReSharper disable InconsistentNaming

namespace ZonkaZombies.Util
{
    public static class LayerConstants
    {
        //layer name
        public const string ENEMY_LAYER_NAME = "Enemy";
        public const string PLAYER_CHARACTER_LAYER_NAME = "PlayerCharacter";
        public const string FLOOR_LAYER_NAME = "Floor";

        //layer index
        public static readonly int ENEMY_LAYER = LayerMask.NameToLayer(ENEMY_LAYER_NAME);
        public static readonly int FLOOR_LAYER = LayerMask.NameToLayer(FLOOR_LAYER_NAME);
        public static readonly int PLAYER_CHARACTER_LAYER = LayerMask.NameToLayer(PLAYER_CHARACTER_LAYER_NAME);
    }

    public static class TagConstants
    {
        public const string PLAYER_DAMAGER = "PlayerDamager";
        public const string PLAYER = "Player";
    }

    public static class SceneConstants
    {
        public const string GAME_OVER_SCENE_NAME = "GameOver";
        public const string MANY_ENEMIES_VS_CHARACTER = "ManyEnemiesVsCharacter";
        public const string PLAYER_WIN_SCENE_NAME = "PlayerWin";
        public const string P1P2_MANY_ENEMIES_VS_CHARACTER = "P1P2ManyEnemiesVsCharacter";
    }

    public static class SharedAnimatorParameters
    {
        public static readonly int WALKING_ID = Animator.StringToHash("Walking");
        public static readonly int PUNCH_ID = Animator.StringToHash("Punch");
    }
}
