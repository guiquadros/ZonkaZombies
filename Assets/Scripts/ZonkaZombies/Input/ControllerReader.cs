using UnityInput = UnityEngine.Input;

namespace ZonkaZombies.Input
{
    public sealed class ControllerReader : InputReader
    {
        internal ControllerReader(MappingKeys mapping) : base(mapping, true)
        {
            IsAController = true;

            // Create the input keys into the Memento object, to be used later
            SavedData.CreateState(MappingKeys.LeftTrigger, LeftTrigger());
            SavedData.CreateState(MappingKeys.RightTrigger, RightTrigger());
        }

        public override void SaveState()
        {
            // Save the input's current data into the Memento object
            SavedData.SetState(MappingKeys.LeftTrigger, LeftTrigger());
            SavedData.SetState(MappingKeys.RightTrigger, RightTrigger());
        }

        #region TRIGGERS
        /// <summary>
        /// Value between 0 (released) and 1 (pressed).
        /// </summary>
        public override float LeftTrigger()
        {
            return UnityInput.GetAxisRaw(MappingKeys.LeftTrigger);
        }

        /// <summary>
        /// Returns TRUE if the trigger is being fully pressed.
        /// </summary>
        public override bool LeftTriggerDown()
        {
            return PreviousLeftTrigger() < 1f && LeftTrigger() >= 1f;
        }

        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public override bool LeftTriggerUp()
        {
            return PreviousLeftTrigger() > 0f && LeftTrigger() <= 0f;
        }

        /// <summary>
        /// Value between 0 (released) and 1 (pressed).
        /// </summary>
        public override float RightTrigger()
        {
            return UnityInput.GetAxisRaw(MappingKeys.RightTrigger);
        }

        /// <summary>
        /// Returns TRUE if the trigger is being fully pressed.
        /// </summary>
        public override bool RightTriggerDown()
        {
            return PreviousRightTrigger() < 1f && RightTrigger() >= 1f;
        }

        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public override bool RightTriggerUp()
        {
            return PreviousRightTrigger() > 0f && RightTrigger() <= 0f;
        }


        /// /// <summary>
        /// Value between 0 (released) and 1 (pressed).
        /// </summary>
        public float PreviousRightTrigger()
        {
            return SavedData.GetState<float>(MappingKeys.RightTrigger);
        }

        /// <summary>
        /// Value between 0 (released) and 1 (pressed).
        /// </summary>
        /// <summary>
        /// Returns the previous of the LeftTrigger.
        /// </summary>
        /// <returns>The previous of the LeftTrigger</returns>
        public float PreviousLeftTrigger()
        {
            return SavedData.GetState<float>(MappingKeys.LeftTrigger);
        }
        #endregion
    }
}