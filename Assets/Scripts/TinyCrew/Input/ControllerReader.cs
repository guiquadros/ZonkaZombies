using System;
using UnityInput = UnityEngine.Input;

namespace TinyCrew.Input
{
    public class ControllerReader : InputReader
    {
        private const float TriggersTolerance = .05f;

        internal ControllerReader(MappingKeys mapping) : base(mapping)
        {
            IsAController = true;
        }

        #region MEMENTO

        internal override void OnCreateStates()
        {
            CreateState(MappingKeys.LeftTrigger, LeftTrigger());
            CreateState(MappingKeys.RightTrigger, RightTrigger());
        }

        protected override void OnSaveState()
        {
            SaveState(MappingKeys.LeftTrigger, LeftTrigger());
            SaveState(MappingKeys.RightTrigger, RightTrigger());
        }

        #endregion

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
            return LoadState<float>(MappingKeys.LeftTrigger) < TriggersTolerance && Math.Abs(LeftTrigger() - 1.0f) < TriggersTolerance;
        }
        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public override bool LeftTriggerUp()
        {
            return LoadState<float>(MappingKeys.LeftTrigger) > TriggersTolerance && LeftTrigger() < TriggersTolerance;
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
            return LoadState<float>(MappingKeys.RightTrigger) < TriggersTolerance && Math.Abs(RightTrigger() - 1.0f) < TriggersTolerance;
        }
        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public override bool RightTriggerUp()
        {
            return LoadState<float>(MappingKeys.RightTrigger) > TriggersTolerance && RightTrigger() < TriggersTolerance;
        }

        #endregion
    }
}