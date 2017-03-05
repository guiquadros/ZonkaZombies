using UnityEngine;

namespace Assets.Scripts.TinyCrew.ScriptableObjects.Input
{
    /// <summary>
    /// BEWARE: Don't rename the variables! We'll lost Scriptable Objects' data if you do that.
    /// </summary>
    [CreateAssetMenu(fileName = "NewMappingKeys", menuName = "TinyCrew/Input/Create MappingKeys", order = 0)]
    public class MappingKeys : ScriptableObject
    {
        [Header("Sticks"), Tooltip("Left horizontal stick")]
        public string LeftHorizontalStick;
        [Tooltip("Left vertical stick")]
        public string LeftVerticalStick;
        [Tooltip("Right horizontal stick")]
        public string rightStickHorizontal;
        [Tooltip("Right vertical stick")]
        public string RightStickVertical;

        [Header("Directional pad (D-pad)")]
        public string dPadHorizontal;
        public string dPadVertical;

        [Header("Axis Triggers"), Tooltip("Right Trigger (RT)")]
        public string RightTrigger;
        [Tooltip("Left Trigger (LT)")]
        public string LeftTrigger;

        [Header("Buttons")]
        public string LeftStickButton;
        public string RightStickButton;

        public string A;
        public string B;
        public string X;
        public string Y;

        public string Back;
        public string Start;

        [Header("Bumpers"), Tooltip("Left Bumper (LeftBumper)")]
        public string LeftBumper;
        [Tooltip("Right Bumper (RightBumper)")]
        public string RightBumper;
    }
}
