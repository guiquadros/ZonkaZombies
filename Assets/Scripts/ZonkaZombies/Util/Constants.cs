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
        public static readonly int SCENERY_LAYER = LayerMask.NameToLayer(ENEMY_LAYER_NAME);
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
        //menu input debug
        public const string MENU_PROTOTYPE = "MenuPrototype";
        public const string INPUT_DEBUGGER = "InputDebugger";
        
        //singleplayer
        public const string P1_PLAYER_CHARACTER_MOVEMENT = "P1PlayerCharacterMovement";
        public const string P1_ENEMY_MOVEMENT_AND_PURSUIT = "P1EnemyMovementandPursuit";
        public const string P1_ENEMY_VS_CHARACTER = "P1EnemyVsCharacter";
        public const string P1_MANY_ENEMIES_VS_CHARACTER = "P1ManyEnemiesVsCharacter";
        public const string P1_FIELD_OF_VISION = "P1FieldOfVision";
        public const string P1_INTERACTABLE_SYSTEM = "P1InteractableSystem";
        public const string P1_FULL_SCENERY = "P1FullScenery";

        //multiplayer
        public const string P2_MOVEMENT = "P2Movement";
        public const string P2_MANY_ENEMIES_VS_CHARACTER = "P2ManyEnemiesVsCharacter";
        public const string P2_INTERACTABLE_SYSTEM_SPLITSCREEN = "P2InteractableSystemSplitscreen";
        public const string P2_FULL_SCENERY = "P2FullScenery";

        //win lose
        public const string GAME_OVER_SCENE_NAME = "GameOver";
        public const string PLAYER_WIN_SCENE_NAME = "PlayerWin";
    }

    public static class SharedAnimatorParameters
    {
        public static readonly int WALKING_ID = Animator.StringToHash("Walking");
        public static readonly int PUNCH_ID = Animator.StringToHash("Punch");
    }
}
