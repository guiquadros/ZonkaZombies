using UnityInput = UnityEngine.Input;

namespace ZonkaZombies.Input
{
    public sealed class ControllerReader : InputReader
    {
        internal ControllerReader(MappingKeys mapping) : base(mapping, true)
        {
            IsAController = true;

            // Create the input keys into the Memento object, to be used later
            SavedData.CreateState(MappingKeys.LeftTrigger, LeftTriggerValue());
            SavedData.CreateState(MappingKeys.RightTrigger, RightTriggerValue());
        }

        public override void SaveState()
        {
            // Save the input's current data into the Memento object
            SavedData.SetState(MappingKeys.LeftTrigger, LeftTriggerValue());
            SavedData.SetState(MappingKeys.RightTrigger, RightTriggerValue());
        }

        #region TRIGGERS
        /// <summary>
        /// Value between 0 (released) and 1 (pressed).
        /// </summary>
        public override float LeftTriggerValue()
        {
            return UnityInput.GetAxisRaw(MappingKeys.LeftTrigger);
        }

        /// <summary>
        /// Returns TRUE if the trigger is being fully pressed.
        /// </summary>
        public override bool LeftTriggerDown()
        {
            return PreviousLeftTriggerValue() < 1f && LeftTriggerValue() >= 1f;
        }

        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public override bool LeftTriggerUp()
        {
            return PreviousLeftTriggerValue() > 0f && LeftTriggerValue() <= 0f;
        }

        /// <summary>
        /// Value between 0 (released) and 1 (pressed).
        /// </summary>
        public override float RightTriggerValue()
        {
            return UnityInput.GetAxisRaw(MappingKeys.RightTrigger);
        }

        /// <summary>
        /// Returns TRUE if the trigger is being fully pressed.
        /// </summary>
        public override bool RightTriggerDown()
        {
            return PreviousRightTriggerValue() < 1f && RightTriggerValue() >= 1f;
        }

        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public override bool RightTriggerUp()
        {
            return PreviousRightTriggerValue() > 0f && RightTriggerValue() <= 0f;
        }

        public override bool RightTrigger()
        {
            return RightTriggerValue() >= 1.0f;
        }
        
        public override bool LeftTrigger()
        {
            return LeftTriggerValue() >= 1.0f;
        }
        #endregion
    }
}