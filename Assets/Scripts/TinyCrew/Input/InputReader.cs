using UnityEngine;

namespace TinyCrew.Input
{
    /// <summary>
    /// This class is responsible for providing a simplified reading of input for generic keys in high level, accessing those as if those were a Xbox controller.
    /// </summary>
    public abstract class InputReader
    {
        internal readonly MappingKeys MappingKeys;

        public bool IsAController { get; protected set; }

        internal InputReader(MappingKeys mapping)
        {
            MappingKeys = mapping;
        }

        #region BUTTONS

        public abstract bool A();
        public abstract bool ADown();
        public abstract bool AUp();

        public abstract bool B();
        public abstract bool BDown();
        public abstract bool BUp();

        public abstract bool X();
        public abstract bool XDown();
        public abstract bool XUp();

        public abstract bool Y();
        public abstract bool YDown();
        public abstract bool YUp();

        public abstract bool Back();
        public abstract bool Start();

        public abstract bool LeftStickButton();
        public abstract bool LeftStickButtonDown();
        public abstract bool LeftStickButtonUp();

        public abstract bool RightStickButton();
        public abstract bool RightStickButtonDown();
        public abstract bool RightStickButtonUp();

        #endregion

        #region BUMPERS

        public abstract bool LeftBumper();
        public abstract bool LeftBumperDown();
        public abstract bool LeftBumperUp();

        public abstract bool RightBumper();
        public abstract bool RightBumperDown();
        public abstract bool RightBumperUp();

        #endregion

        #region ANALOG STICKS

        public abstract Vector2 LeftAnalogStick();

        /// <summary>
        /// X value between -1 and 1.
        /// </summary>
        public abstract float LeftAnalogStickHorizontal();

        /// <summary>
        /// Y value between -1 and 1.
        /// </summary>
        public abstract float LeftAnalogStickVertical();

        public abstract Vector2 RightAnalogStick();

        /// <summary>
        /// X value between -1 and 1.
        /// </summary>
        public abstract float RightAnalogStickHorizontal();

        /// <summary>
        /// Y value between -1 and 1.
        /// </summary>
        public abstract float RightAnalogStickVertical();

        #endregion

        #region DIGITAL PAD (D-PAD)

        public abstract Vector2 DigitalPad();

        /// <summary>
        /// X value between -1 and 1.
        /// </summary>
        public abstract float DigitalPadHorizontal();

        /// <summary>
        /// Y value between -1 and 1.
        /// </summary>
        public abstract float DigitalPadVertical();

        #endregion

        #region TRIGGERS

        /// <summary>
        /// Value between 0 (released) and 1 (pressed).
        /// </summary>
        public abstract float LeftTrigger();

        /// <summary>
        /// Returns TRUE if the trigger is being fully pressed.
        /// </summary>
        public abstract bool LeftTriggerDown();

        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public abstract bool LeftTriggerUp();

        /// <summary>
        /// Value between 0 (released) and 1 (pressed).
        /// </summary>
        public abstract float RightTrigger();

        /// <summary>
        /// Returns TRUE if the trigger is being fully pressed.
        /// </summary>
        public abstract bool RightTriggerDown();

        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public abstract bool RightTriggerUp();

        #endregion
    }
}
