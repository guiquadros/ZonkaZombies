using UnityEngine;
// ReSharper disable InconsistentNaming

namespace ZonkaZombies.Util
{
    public static class LayerConstants
    {
        public static readonly int ENEMY_LAYER = LayerMask.NameToLayer("Enemy");
        public static readonly int PLAYER_CHARACTER_LAYER = LayerMask.NameToLayer("PlayerCharacter");
    }
}
