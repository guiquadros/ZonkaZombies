using ZonkaZombies.Prototype.Characters.PlayerCharacter;

namespace ZonkaZombies.Prototype.Characters.Player
{
    public class PoliceOfficerBehavior : Player
    {
        protected override void Awake()
        {
            base.Awake();

            Type = PlayerCharacterType.PoliceOfficer;
        }
    }
}