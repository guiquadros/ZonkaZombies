using System;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace ZonkaZombies.Input
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
            return !IsTriggerPressed(PreviousLeftTrigger()) && IsTriggerPressed(LeftTrigger());
        }

        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public override bool LeftTriggerUp()
        {
            return IsTriggerUp(PreviousLeftTrigger(), LeftTrigger());
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
            return !IsTriggerPressed(PreviousRightTrigger()) && IsTriggerPressed(RightTrigger());
        }

        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public override bool RightTriggerUp()
        {
            return IsTriggerUp(PreviousRightTrigger(), RightTrigger());
        }

        /// <summary>
        /// Returns True if the value of the trigger (axisRawValue) minus the max value of the trigger (1.0f) is less than the TriggersTolerance.
        /// </summary>
        /// <param name="axisRawValue">Value of the trigger</param>
        /// <returns></returns>
        private static bool IsTriggerPressed(float axisRawValue)
        {
            return Math.Abs(axisRawValue - 1.0f) < TriggersTolerance;
        }

        /// <summary>
        /// Returns the previous of the RightTrigger.
        /// </summary>
        /// <returns>The previous of the RightTrigger</returns>
        public override float PreviousRightTrigger()
        {
            return SavedData.GetState<float>(MappingKeys.RightTrigger);
        }

        /// <summary>
        /// Returns the previous of the LeftTrigger.
        /// </summary>
        /// <returns>The previous of the LeftTrigger</returns>
        public override float PreviousLeftTrigger()
        {
            return SavedData.GetState<float>(MappingKeys.LeftTrigger);
        }

        /// <summary>
        /// Returns TRUE if the Trigger is up.
        /// </summary>
        /// <param name="previousTriggerValue">The previous value of the trigger</param>
        /// <param name="currentTriggerValue">The current value of the trigger</param>
        /// <returns></returns>
        private static bool IsTriggerUp(float previousTriggerValue, float currentTriggerValue)
        {
            return previousTriggerValue > TriggersTolerance && currentTriggerValue < TriggersTolerance;
        }
        #endregion
    }
}