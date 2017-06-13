namespace ZonkaZombies.Characters.Player.Behaviors
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