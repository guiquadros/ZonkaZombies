using System;
using Assets.Scripts.TinyCrew.ScriptableObjects.Input;
using UnityEngine;
using UnityInput = UnityEngine.Input;
// ReSharper disable InconsistentNaming

namespace Assets.Scripts.TinyCrew.Input
{
    /// <summary>
    /// This class is responsible for providing a simplified reading of input for generic keys in high level, accessing those as if those were a Xbox controller.
    /// </summary>
    public class InputReader
    {
        private const float TRIGGERS_TOLERANCE = .05f;

        private readonly MappingKeys _mappingKeys;
        
        public InputReader(MappingKeys mapping)
        {
            _mappingKeys = mapping;
        }

        #region BUTTONS

        public bool A()
        {
            return UnityInput.GetButton(_mappingKeys.A);
        }
        public bool ADown()
        {
            return UnityInput.GetButtonDown(_mappingKeys.A);
        }
        public bool AUp()
        {
            return UnityInput.GetButtonUp(_mappingKeys.A);
        }

        public bool B()
        {
            return UnityInput.GetButton(_mappingKeys.B);
        }
        public bool BDown()
        {
            return UnityInput.GetButtonDown(_mappingKeys.B);
        }
        public bool BUp()
        {
            return UnityInput.GetButtonUp(_mappingKeys.B);
        }

        public bool X()
        {
            return UnityInput.GetButton(_mappingKeys.X);
        }
        public bool XDown()
        {
            return UnityInput.GetButtonDown(_mappingKeys.X);
        }
        public bool XUp()
        {
            return UnityInput.GetButtonUp(_mappingKeys.X);
        }

        public bool Y()
        {
            return UnityInput.GetButton(_mappingKeys.Y);
        }
        public bool YDown()
        {
            return UnityInput.GetButtonDown(_mappingKeys.Y);
        }
        public bool YUp()
        {
            return UnityInput.GetButtonUp(_mappingKeys.Y);
        }

        public bool Back()
        {
            return UnityInput.GetButton(_mappingKeys.Back);
        }
        public bool Start()
        {
            return UnityInput.GetButton(_mappingKeys.Start);
        }

        public bool LeftStickButton()
        {
            return UnityInput.GetButton(_mappingKeys.LeftStickButton);
        }
        public bool LeftStickButtonDown()
        {
            return UnityInput.GetButtonDown(_mappingKeys.LeftStickButton);
        }
        public bool LeftStickButtonUp()
        {
            return UnityInput.GetButtonUp(_mappingKeys.LeftStickButton);
        }

        public bool RightStickButton()
        {
            return UnityInput.GetButton(_mappingKeys.RightStickButton);
        }
        public bool RightStickButtonDown()
        {
            return UnityInput.GetButtonDown(_mappingKeys.RightStickButton);
        }
        public bool RightStickButtonUp()
        {
            return UnityInput.GetButtonUp(_mappingKeys.RightStickButton);
        }

        #endregion

        #region BUMPERS

        public bool LeftBumper()
        {
            return UnityInput.GetButton(_mappingKeys.LeftBumper);
        }
        public bool LeftBumperDown()
        {
            return UnityInput.GetButtonDown(_mappingKeys.LeftBumper);
        }
        public bool LeftBumperUp()
        {
            return UnityInput.GetButtonUp(_mappingKeys.LeftBumper);
        }

        public bool RightBumper()
        {
            return UnityInput.GetButton(_mappingKeys.RightBumper);
        }
        public bool RightBumperDown()
        {
            return UnityInput.GetButtonDown(_mappingKeys.RightBumper);
        }
        public bool RightBumperUp()
        {
            return UnityInput.GetButtonUp(_mappingKeys.RightBumper);
        }

        #endregion

        #region ANALOG STICKS

        public Vector2 LeftAnalogStick()
        {
            return new Vector2(LeftAnalogStickHorizontal(), LeftAnalogStickVertical());
        }
        /// <summary>
        /// X value between -1 and 1.
        /// </summary>
        public float LeftAnalogStickHorizontal()
        {
            return UnityInput.GetAxisRaw(_mappingKeys.LeftHorizontalStick);
        }
        /// <summary>
        /// Y value between -1 and 1.
        /// </summary>
        public float LeftAnalogStickVertical()
        {
            return UnityInput.GetAxisRaw(_mappingKeys.LeftVerticalStick);
        }

        public Vector2 RightAnalogStick()
        {
            return new Vector2(RightAnalogStickHorizontal(), RightAnalogStickVertical());
        }
        /// <summary>
        /// X value between -1 and 1.
        /// </summary>
        public float RightAnalogStickHorizontal()
        {
            return UnityInput.GetAxisRaw(_mappingKeys.rightStickHorizontal);
        }
        /// <summary>
        /// Y value between -1 and 1.
        /// </summary>
        public float RightAnalogStickVertical()
        {
            return UnityInput.GetAxisRaw(_mappingKeys.RightStickVertical);
        }

        #endregion

        #region DIGITAL PAD (D-PAD)

        public Vector2 DigitalPad()
        {
            return new Vector2(DigitalPadHorizontal(), DigitalPadVertical());
        }
        /// <summary>
        /// X value between -1 and 1.
        /// </summary>
        public float DigitalPadHorizontal()
        {
            return UnityInput.GetAxisRaw(_mappingKeys.dPadHorizontal);
        }
        /// <summary>
        /// Y value between -1 and 1.
        /// </summary>
        public float DigitalPadVertical()
        {
            return UnityInput.GetAxisRaw(_mappingKeys.dPadVertical);
        }

        #endregion

        #region TRIGGERS

        /// <summary>
        /// Value between 0 (released) and 1 (pressed).
        /// </summary>
        public float LeftTrigger()
        {
            return UnityInput.GetAxisRaw(_mappingKeys.LeftTrigger);
        }
        /// <summary>
        /// Returns TRUE if the trigger is being fully pressed.
        /// </summary>
        public bool LeftTriggerDown()
        {
            return Math.Abs(LeftTrigger() - 1.0f) < TRIGGERS_TOLERANCE;
        }
        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public bool LeftTriggerUp()
        {
            return Math.Abs(LeftTrigger()) < TRIGGERS_TOLERANCE;
        }

        /// <summary>
        /// Value between 0 (released) and 1 (pressed).
        /// </summary>
        public float RightTrigger()
        {
            return UnityInput.GetAxisRaw(_mappingKeys.RightTrigger);
        }
        /// <summary>
        /// Returns TRUE if the trigger is being fully pressed.
        /// </summary>
        public bool RightTriggerDown()
        {
            return Math.Abs(RightTrigger() - 1.0f) < TRIGGERS_TOLERANCE;
        }
        /// <summary>
        /// Returns TRUE if the trigger is being fully released.
        /// </summary>
        public bool RightTriggerUp()
        {
            return Math.Abs(RightTrigger()) < TRIGGERS_TOLERANCE;
        }

        #endregion
    }
}
