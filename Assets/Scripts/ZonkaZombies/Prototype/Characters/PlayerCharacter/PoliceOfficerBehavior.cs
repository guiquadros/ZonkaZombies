namespace ZonkaZombies.Prototype.Characters.PlayerCharacter
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