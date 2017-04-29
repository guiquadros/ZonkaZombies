using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ZonkaZombies.Input.Test
{
    public class InputDebugger : MonoBehaviour
    {
        [SerializeField]
        private InputType _inputType;

        [SerializeField]
        private Text _debugText;

        private InputReader _inputReader;

        private void Awake()
        {
            if (!_debugText)
            {
                Debug.LogException(new MissingReferenceException("debugText can't be null!"), this);
                Application.Quit(); //Forces application to finish
            }
        }

        private void Start()
        {
            _inputReader = InputFactory.Create(_inputType);
        }

        private void FixedUpdate()
        {
            StringBuilder sb = new StringBuilder();

            DebugButtonsState(sb);
            sb.AppendLine();

            DebugBumpersState(sb);
            sb.AppendLine();

            DebugTriggersState(sb);
            sb.AppendLine();

            DebugDigitalPadState(sb);
            sb.AppendLine();

            DebugAnalogSticksState(sb);

            _debugText.text = sb.ToString();

            _inputReader.SaveState();
        }

        private void DebugButtonsState(StringBuilder sb)
        {
            sb.Append("Buttons: ");

            if (_inputReader.X())
                sb.Append("X ");
            if (_inputReader.A())
                sb.Append("A ");
            if (_inputReader.B())
                sb.Append("B ");
            if (_inputReader.Y())
                sb.Append("Y ");
            if (_inputReader.Start())
                sb.Append("START ");
            if (_inputReader.Back())
                sb.Append("BACK ");
            if (_inputReader.LeftStickButton())
                sb.Append("LSB ");
            if (_inputReader.RightStickButton())
                sb.Append("RSB");
        }

        private void DebugBumpersState(StringBuilder sb)
        {
            sb.Append("Bumpers: ");

            if (_inputReader.LeftBumper())
                sb.Append("LB ");
            if (_inputReader.RightBumper())
                sb.Append("RB");
        }

        private void DebugTriggersState(StringBuilder sb)
        {
            sb.Append("Left Trigger: ");
            sb.Append(_inputReader.LeftTriggerValue());

            sb.AppendLine();

            sb.Append("Right Trigger: ");
            sb.Append(_inputReader.RightTriggerValue());
        }

        private void DebugAnalogSticksState(StringBuilder sb)
        {
            sb.Append("Left Analog Stick: ");
            sb.Append(_inputReader.LeftAnalogStick());

            sb.AppendLine();

            sb.Append("Right Analog Stick: ");
            sb.Append(_inputReader.RightAnalogStick());
        }

        private void DebugDigitalPadState(StringBuilder sb)
        {
            sb.Append("D-pad: ");
            sb.Append(_inputReader.DigitalPad());
        }

    }
}