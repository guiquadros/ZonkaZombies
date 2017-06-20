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
        public const string ENEMY_DAMAGE_LAYER_NAME = "EnemyDamager";

        //layer index
        public static readonly int SCENERY_LAYER = LayerMask.NameToLayer(ENEMY_LAYER_NAME);
        public static readonly int ENEMY_LAYER = LayerMask.NameToLayer(ENEMY_LAYER_NAME);
        public static readonly int FLOOR_LAYER = LayerMask.NameToLayer(FLOOR_LAYER_NAME);
        public static readonly int PLAYER_CHARACTER_LAYER = LayerMask.NameToLayer(PLAYER_CHARACTER_LAYER_NAME);
        public static readonly int ENEMY_DAMAGER_LAYER = LayerMask.NameToLayer(ENEMY_DAMAGE_LAYER_NAME);
    }

    public static class TagConstants
    {
        public const string ENEMY_DAMAGER = "EnemyDamager";
        public const string PLAYER_DAMAGER = "PlayerDamager";
        public const string PLAYER = "Player";
    }

    public static class SceneConstants
    {
        //general
        public const string PERSISTENT = "Persistent";
        public const string HALL_FIRST_FLOOR_NAME = "HallFirstFloor";
        public const string DIALOGUE_SCIENTIST_NAME = "DialogueScientist";

        //win lose
        public const string GAME_OVER_SCENE_NAME = "GameOver";
        public const string PLAYER_WIN_SCENE_NAME = "PlayerWin";
    }

    public static class SharedAnimatorParameters
    {
        public static readonly int CALL_ELEVATOR_ID = Animator.StringToHash("CallElevator");
        public static readonly int HUD_DAMAGE_ID = Animator.StringToHash("HudDamage");
    }

    public static class PlayerAnimatorParameters
    {
        public static readonly int WALKING = Animator.StringToHash("Walking");
        public static readonly int PUNCH = Animator.StringToHash("Punch");
        public static readonly int MOVEMENT_DIRECTION = Animator.StringToHash("MovementDirection");
        public static readonly int IDLE = Animator.StringToHash("Idle");
        public static readonly int DAMAGE_FRONT = Animator.StringToHash("DamageFront");
        public static readonly int DAMAGE_BACK = Animator.StringToHash("DamageBack");
        public static readonly int FORCE_IDLE = Animator.StringToHash("ForceIdle");
    }

    public static class EnemyAnimatorParameters
    {
        public static readonly int IS_MOVING = Animator.StringToHash("IsMoving");
        public static readonly int ATTACK = Animator.StringToHash("Attack");
        public static readonly int TAKE_DAMAGE = Animator.StringToHash("TakeDamage");
        public static readonly int IS_DEAD = Animator.StringToHash("IsDead");
        public static readonly int DEATH_SPEED = Animator.StringToHash("DeathSpeed");
    }

    public static class ScriptableObjectsConstants
    {
        public const string SPAWN_ENEMIES_ASSET_PATH = "Assets/Data/SpawnPointsZombies{0}.asset";
    } 
}
