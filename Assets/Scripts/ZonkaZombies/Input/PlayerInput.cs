using System;
using System.Diagnostics.CodeAnalysis;

namespace ZonkaZombies.Input
{
    [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
    [SuppressMessage("ReSharper", "UseNameofExpression")]
    public static class PlayerInput
    {
        private static readonly InputReader _inputReaderController1 = InputFactory.Create(InputType.Controller1);
        private static readonly InputReader _inputReaderController2 = InputFactory.Create(InputType.Controller2);
        private static readonly InputReader _inputReaderKeyboard = InputFactory.Create(InputType.Keyboard);

        public static InputReader InputReaderController1
        {
            get { return _inputReaderController1; }
        }

        public static InputReader InputReaderController2
        {
            get { return _inputReaderController2; }
        }

        public static InputReader InputReaderKeyboard
        {
            get { return _inputReaderKeyboard; }
        }

        public static InputReader GetInputReader(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.Controller1:
                    return InputReaderController1;
                case InputType.Controller2:
                    return InputReaderController2;
                case InputType.Keyboard:
                    return InputReaderKeyboard;
                default:
                    throw new ArgumentOutOfRangeException("inputType", inputType, null);
            }
        }
    }
}