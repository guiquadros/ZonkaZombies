using UnityEngine;
using ZonkaZombies.Characters.Enemy.EnemyIA;
using ZonkaZombies.Characters.Player.Behaviors;

namespace ZonkaZombies.Messaging.Messages.UI
{
#region BASE MESSAGE

    public abstract class BasePlayerMessage
    {
        public readonly Player Player;

        protected BasePlayerMessage(Player player)
        {
            Player = player;
        }
    }

    public abstract class BaseEnemyMessage
    {
        public readonly GenericEnemy Enemy;

        protected BaseEnemyMessage(GenericEnemy enemy)
        {
            Enemy = enemy;
        }
    }

#endregion

#region Player Messages

    public sealed class OnPlayerHasBornMessage : BasePlayerMessage
    {
        public OnPlayerHasBornMessage(Player player) : base(player) { }
    }
    
    public sealed class OnPlayerDeadMessage : BasePlayerMessage
    {
        public OnPlayerDeadMessage(Player player) : base(player) { }
    }

#endregion

#region Enemy Messages

    public class OnEnemyHasBornMessage : BaseEnemyMessage
    {
        public OnEnemyHasBornMessage(GenericEnemy enemy) : base(enemy) { }
    }

    public class OnEnemyDeadMessage : BaseEnemyMessage
    {
        public OnEnemyDeadMessage(GenericEnemy enemy) : base(enemy) { }
    }

    #endregion

#region GAMEPLAY MESSAGES

    public struct OnAllPlayersAreDead { }

    #endregion

#region MAIN MENU MESSAGES
    public class MoveCameraMessage
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public float Duration = 2;
    }
#endregion
}