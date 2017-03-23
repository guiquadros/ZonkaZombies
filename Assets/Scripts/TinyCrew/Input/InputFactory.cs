using System;
using UnityEngine;

namespace TinyCrew.Input
{
    public static class InputFactory
    {
        public static InputReader Create(InputType inputType)
        {
            string inputFilePath = ToFileName(inputType);

            MappingKeys mappingKeys = FindMappingKeysFromResources(inputFilePath);

            InputReader inputReader = new ControllerReader(mappingKeys);

            return inputReader;
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
                default:
                    throw new ArgumentOutOfRangeException("inputType", inputType, "InputType's value is not valid!");
            }
        }

        /// <summary>
        /// Find, deserialize and returns an instance of MappingKeys containing the correct input's mapping, according to the parameter 'inputFilePath'.
        /// </summary>
        private static MappingKeys FindMappingKeysFromResources(string inputFilePath)
        {
            TextAsset fileTextAsset = Resources.Load<TextAsset>(inputFilePath);

            if (!fileTextAsset)
            {
                throw new NullReferenceException(string.Format("\"{0}\" doesn't exist inside Resources folder!", inputFilePath));
            }

            if (string.IsNullOrEmpty(fileTextAsset.text))
            {
                throw new NullReferenceException(string.Format("The file \"{0}\" is blank!", inputFilePath));
            }

            MappingKeys mappingKeys = JsonUtility.FromJson<MappingKeys>(fileTextAsset.text);

            return mappingKeys;
        }
    }
}