namespace ZonkaZombies.Characters.Player.Util
{
    public class CharacterStateMessage
    {
        public static CharacterStateMessage EnableMovement = CreateInstance(CharacterMechanicType.Movement, true);
        public static CharacterStateMessage DisableMovement = CreateInstance(CharacterMechanicType.Movement, false);

        public static CharacterStateMessage EnablePunch = CreateInstance(CharacterMechanicType.Punch, true);
        public static CharacterStateMessage DisablePunch = CreateInstance(CharacterMechanicType.Punch, false);

        public static CharacterStateMessage EnableRotation = CreateInstance(CharacterMechanicType.Rotation, true);
        public static CharacterStateMessage DisableRotation = CreateInstance(CharacterMechanicType.Rotation, false);

        public static CharacterStateMessage EnableInteraction = CreateInstance(CharacterMechanicType.Interaction, true);
        public static CharacterStateMessage DisableInteraction = CreateInstance(CharacterMechanicType.Interaction, false);

        public CharacterMechanicType Type { get; set; }
        public bool Value { get; set; }
        
        private CharacterStateMessage(CharacterMechanicType type, bool value)
        {
            Type = type;
            Value = value;
        }

        private static CharacterStateMessage CreateInstance(CharacterMechanicType type, bool value)
        {
            return new CharacterStateMessage(type, value);
        }
    }
}