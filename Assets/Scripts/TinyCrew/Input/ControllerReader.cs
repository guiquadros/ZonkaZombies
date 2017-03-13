using System;
using UnityEngine;
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

        #region BUTTONS

        public override bool A()
        {
            return UnityInput.GetButton(MappingKeys.A);
        }
        public override bool ADown()
        {
            return UnityInput.GetButtonDown(MappingKeys.A);
        }
        public override bool AUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.A);
        }

        public override bool B()
        {
            return UnityInput.GetButton(MappingKeys.B);
        }
        public override bool BDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.B);
        }
        public override bool BUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.B);
        }

        public override bool X()
        {
            return UnityInput.GetButton(MappingKeys.X);
        }
        public override bool XDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.X);
        }
        public override bool XUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.X);
        }

        public override bool Y()
        {
            return UnityInput.GetButton(MappingKeys.Y);
        }
        public override bool YDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.Y);
        }
        public override bool YUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.Y);
        }

        public override bool Back()
        {
            return UnityInput.GetButton(MappingKeys.Back);
        }
        public override bool Start()
        {
            return UnityInput.GetButton(MappingKeys.Start);
        }

        public override bool LeftStickButton()
        {
            return UnityInput.GetButton(MappingKeys.LeftStickButton);
        }
        public override bool LeftStickButtonDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.LeftStickButton);
        }
        public override bool LeftStickButtonUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.LeftStickButton);
        }

        public override bool RightStickButton()
        {
            return UnityInput.GetButton(MappingKeys.RightStickButton);
        }
        public override bool RightStickButtonDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.RightStickButton);
        }
        public override bool RightStickButtonUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.RightStickButton);
        }

        #endregion

        #region BUMPERS

        public override bool LeftBumper()
        {
            return UnityInput.GetButton(MappingKeys.LeftBumper);
        }
        public override bool LeftBumperDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.LeftBumper);
        }
        public override bool LeftBumperUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.LeftBumper);
        }

        public override bool RightBumper()
        {
            return UnityInput.GetButton(MappingKeys.RightBumper);
        }
        public override bool RightBumperDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.RightBumper);
        }
        public override bool RightBumperUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.RightBumper);
        }

        #endregion

        #region ANALOG STICKS

        public override Vector2 LeftAnalogStick()
        {
            return new Vector2(LeftAnalogStickHorizontal(), LeftAnalogStickVertical());
        }
        /// <summary>
        /// X value between -1 and 1.
        /// </summary>
        public override float LeftAnalogStickHorizontal()
        {
            return UnityInput.GetAxisRaw(MappingKeys.LeftStickHorizontal);
        }
        /// <summary>
        /// Y value between -1 and 1.
        /// </summary>
        public override float LeftAnalogStickVertical()
        {
            return UnityInput.GetAxisRaw(MappingKeys.LeftStickVertical);
        }

        public override Vector2 RightAnalogStick()
        {
            return new Vector2(RightAnalogStickHorizontal(), RightAnalogStickVertical());
        }
        /// <summary>
        /// X value between -1 and 1.
        /// </summary>
        public override float RightAnalogStickHorizontal()
        {
            return UnityInput.GetAxisRaw(MappingKeys.RightStickHorizontal);
        }
        /// <summary>
        /// Y value between -1 and 1.
        /// </summary>
        public override float RightAnalogStickVertical()
        {
            return UnityInput.GetAxisRaw(MappingKeys.RightStickVertical);
        }

        #endregion

        #region DIGITAL PAD (D-PAD)

        public override Vector2 DigitalPad()
        {
            return new Vector2(DigitalPadHorizontal(), DigitalPadVertical());
        }
        /// <summary>
        /// X value between -1 and 1.
        /// </summary>
        public override float DigitalPadHorizontal()
        {
            return UnityInput.GetAxisRaw(MappingKeys.DPadHorizontal);
        }
        /// <summary>
        /// Y value between -1 and 1.
        /// </summary>
        public override float DigitalPadVertical()
        {
            return UnityInput.GetAxisRaw(MappingKeys.DPadVertical);
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
            return Math.Abs(LeftTrigger() - 1.0f) < TriggersTolerance;
        }
        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public override bool LeftTriggerUp()
        {
            return Math.Abs(LeftTrigger()) < TriggersTolerance;
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
            return Math.Abs(RightTrigger() - 1.0f) < TriggersTolerance;
        }
        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public override bool RightTriggerUp()
        {
            return Math.Abs(RightTrigger()) < TriggersTolerance;
        }

        #endregion
    }
}
