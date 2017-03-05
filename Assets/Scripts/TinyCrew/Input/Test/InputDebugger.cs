using System.Text;
using Assets.Scripts.TinyCrew.ScriptableObjects.Input;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.TinyCrew.Input.Test
{
    public class InputDebugger : MonoBehaviour
    {
        [SerializeField]
        private MappingKeys mappingKeys;

        [SerializeField]
        private Text debugText;

        private InputReader inputReader;

        private void Awake()
        {
            if (!debugText)
            {
                Debug.LogException(new MissingReferenceException("debugText can't be null!"), this);
                Application.Quit(); //Forces application to finish
            }
            else if (!mappingKeys)
            {
                Debug.LogException(new MissingReferenceException("mappingKeys can't be null!"), this);
                Application.Quit(); //Forces application to finish
            }
        }

        private void Start()
        {
            inputReader = new InputReader(mappingKeys);
        }

        private void Update()
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

            debugText.text = sb.ToString();
        }

        private void DebugButtonsState(StringBuilder sb)
        {
            sb.Append("Buttons: ");

            if (inputReader.X())
                sb.Append("X ");
            if (inputReader.A())
                sb.Append("A ");
            if (inputReader.B())
                sb.Append("B ");
            if (inputReader.Y())
                sb.Append("Y ");
            if (inputReader.Start())
                sb.Append("START ");
            if (inputReader.Back())
                sb.Append("BACK ");
            if (inputReader.LeftStickButton())
                sb.Append("LSB ");
            if (inputReader.RightStickButton())
                sb.Append("RSB");
        }

        private void DebugBumpersState(StringBuilder sb)
        {
            sb.Append("Bumpers: ");

            if (inputReader.LeftBumper())
                sb.Append("LB ");
            if (inputReader.RightBumper())
                sb.Append("RB");
        }

        private void DebugTriggersState(StringBuilder sb)
        {
            sb.Append("Left Trigger: ");
            sb.Append(inputReader.LeftTrigger());

            sb.AppendLine();

            sb.Append("Right Trigger: ");
            sb.Append(inputReader.RightTrigger());
        }

        private void DebugAnalogSticksState(StringBuilder sb)
        {
            sb.Append("Left Analog Stick: ");
            sb.Append(inputReader.LeftAnalogStick());

            sb.AppendLine();

            sb.Append("Right Analog Stick: ");
            sb.Append(inputReader.RightAnalogStick());
        }

        private void DebugDigitalPadState(StringBuilder sb)
        {
            sb.Append("D-pad: ");
            sb.Append(inputReader.DigitalPad());
        }

    }
}