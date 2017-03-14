using System;
using UnityInput = UnityEngine.Input;

namespace TinyCrew.Input
{
    public sealed class ControllerReader : InputReader
    {
        private const float TriggersTolerance = .05f;

        internal ControllerReader(MappingKeys mapping) : base(mapping)
        {
            IsAController = true;

            // Create the input keys into the Memento object, to be used later
            SavedData.CreateState(MappingKeys.LeftTrigger, LeftTrigger());
            SavedData.CreateState(MappingKeys.RightTrigger, RightTrigger());
        }

        public override void Update()
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
            float lastKnownState = SavedData.GetState<float>(MappingKeys.LeftTrigger);
            return lastKnownState < TriggersTolerance && Math.Abs(LeftTrigger() - 1.0f) < TriggersTolerance;
        }
        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public override bool LeftTriggerUp()
        {
            float lastKnownState = SavedData.GetState<float>(MappingKeys.LeftTrigger);
            return lastKnownState > TriggersTolerance && LeftTrigger() < TriggersTolerance;
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
            float lastKnownState = SavedData.GetState<float>(MappingKeys.RightTrigger);
            return lastKnownState < TriggersTolerance && Math.Abs(RightTrigger() - 1.0f) < TriggersTolerance;
        }
        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public override bool RightTriggerUp()
        {
            float lastKnownState = SavedData.GetState<float>(MappingKeys.RightTrigger);
            return lastKnownState > TriggersTolerance && RightTrigger() < TriggersTolerance;
        }

        #endregion
    }
}