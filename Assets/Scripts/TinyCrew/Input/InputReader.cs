using TinyCrew.Util;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace TinyCrew.Input
{
    /// <summary>
    /// This class is responsible for providing a simplified reading of input for generic keys in high level, accessing those as if those were a Xbox controller.
    /// </summary>
    public abstract class InputReader
    {
        internal readonly MappingKeys MappingKeys;

        public bool IsAController { get; protected set; }

        protected readonly Memento<string> SavedData = new Memento<string>();

        internal InputReader(MappingKeys mapping)
        {
            MappingKeys = mapping;            
        }

        /// <summary>
        /// Updates this InputReader to maintain data between frames. This method needs to be called each frame to updates its internal data to be used in the next frame, correctly.
        /// </summary>
        public abstract void Update();

        #region BUTTONS

        public bool A()
        {
            return UnityInput.GetButton(MappingKeys.A);
        }
        public bool ADown()
        {
            return UnityInput.GetButtonDown(MappingKeys.A);
        }
        public bool AUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.A);
        }

        public bool B()
        {
            return UnityInput.GetButton(MappingKeys.B);
        }
        public bool BDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.B);
        }
        public bool BUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.B);
        }

        public bool X()
        {
            return UnityInput.GetButton(MappingKeys.X);
        }
        public bool XDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.X);
        }
        public bool XUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.X);
        }

        public bool Y()
        {
            return UnityInput.GetButton(MappingKeys.Y);
        }
        public bool YDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.Y);
        }
        public bool YUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.Y);
        }

        public bool Back()
        {
            return UnityInput.GetButton(MappingKeys.Back);
        }
        public bool BackDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.Back);
        }
        public bool BackUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.Back);
        }

        public bool Start()
        {
            return UnityInput.GetButton(MappingKeys.Start);
        }
        public bool StartDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.Start);
        }
        public bool StartUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.Start);
        }

        public bool LeftStickButton()
        {
            return UnityInput.GetButton(MappingKeys.LeftStickButton);
        }
        public bool LeftStickButtonDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.LeftStickButton);
        }
        public bool LeftStickButtonUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.LeftStickButton);
        }

        public bool RightStickButton()
        {
            return UnityInput.GetButton(MappingKeys.RightStickButton);
        }
        public bool RightStickButtonDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.RightStickButton);
        }
        public bool RightStickButtonUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.RightStickButton);
        }

        #endregion

        #region BUMPERS

        public bool LeftBumper()
        {
            return UnityInput.GetButton(MappingKeys.LeftBumper);
        }
        public bool LeftBumperDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.LeftBumper);
        }
        public bool LeftBumperUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.LeftBumper);
        }

        public bool RightBumper()
        {
            return UnityInput.GetButton(MappingKeys.RightBumper);
        }
        public bool RightBumperDown()
        {
            return UnityInput.GetButtonDown(MappingKeys.RightBumper);
        }
        public bool RightBumperUp()
        {
            return UnityInput.GetButtonUp(MappingKeys.RightBumper);
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
            return UnityInput.GetAxisRaw(MappingKeys.LeftStickHorizontal);
        }
        /// <summary>
        /// Y value between -1 and 1.
        /// </summary>
        public float LeftAnalogStickVertical()
        {
            return UnityInput.GetAxisRaw(MappingKeys.LeftStickVertical);
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
            return UnityInput.GetAxisRaw(MappingKeys.RightStickHorizontal);
        }
        /// <summary>
        /// Y value between -1 and 1.
        /// </summary>
        public float RightAnalogStickVertical()
        {
            return UnityInput.GetAxisRaw(MappingKeys.RightStickVertical);
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
            return UnityInput.GetAxisRaw(MappingKeys.DPadHorizontal);
        }
        /// <summary>
        /// Y value between -1 and 1.
        /// </summary>
        public float DigitalPadVertical()
        {
            return UnityInput.GetAxisRaw(MappingKeys.DPadVertical);
        }

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
