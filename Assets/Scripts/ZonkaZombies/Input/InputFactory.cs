using System;
using UnityEngine;

namespace ZonkaZombies.Input
{
    public static class InputFactory
    {
        private const string PathToMappingKeys = "Input/{0}";

        public static InputReader Create(InputType inputType)
        {
            string inputFilePath = ToFileName(inputType);

            MappingKeys mappingKeys = FindMappingKeysFromResources(inputFilePath);

            if (inputType == InputType.Keyboard)
            {
                return new KeyboardReader(mappingKeys);
            }
            else
            {
                return new ControllerReader(mappingKeys);
            }
        }

        /// <summary>
        /// Transforms an InputType into a string containing the name of the correct MappingKeys' file.
        /// </summary>
        private static string ToFileName(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.Controller1:
                    return "controller1";
                case InputType.Controller2:
                    return "controller2";
                case InputType.Keyboard:
                    return "keyboard";
                default:
                    throw new ArgumentOutOfRangeException("inputType", inputType, "InputType's value is not valid!");
            }
        }

        /// <summary>
        /// Find, deserialize and returns an instance of MappingKeys containing the correct input's mapping, according to the parameter 'inputFilePath'.
        /// </summary>
        private static MappingKeys FindMappingKeysFromResources(string inputFileName)
        {
            TextAsset fileTextAsset = Resources.Load<TextAsset>(string.Format(PathToMappingKeys, inputFileName));

            if (!fileTextAsset)
            {
                throw new NullReferenceException(string.Format("\"{0}\" doesn't exist inside Resources folder!", inputFileName));
            }

            if (string.IsNullOrEmpty(fileTextAsset.text))
            {
                throw new NullReferenceException(string.Format("The file \"{0}\" is blank!", inputFileName));
            }

            MappingKeys mappingKeys = JsonUtility.FromJson<MappingKeys>(fileTextAsset.text);

            return mappingKeys;
        }
    }
}