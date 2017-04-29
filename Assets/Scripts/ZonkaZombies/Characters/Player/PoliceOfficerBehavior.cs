using ZonkaZombies.Characters.PlayerCharacter;

namespace ZonkaZombies.Characters.Player
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