using System;
using UnityEngine;

namespace TinyCrew.Input
{
    public static class InputFactory
    {
        public static InputReader Create(InputType inputType)
        {
            string inputFilePath;

            switch (inputType)
            {
                case InputType.Controller1:
                    inputFilePath = "controller1";
                    break;
                case InputType.Controller2:
                    inputFilePath = "controller2";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("inputType", inputType, "InputType's value is not valid!");
            }

            MappingKeys mappingKeys = FindMappingKeysFromResources(inputFilePath);

            InputReader inputReader = new ControllerReader(mappingKeys);

            return inputReader;
        }

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
            else
            {
                Debug.Log(fileTextAsset.text);
            }

            MappingKeys mappingKeys = (MappingKeys) JsonUtility.FromJson(fileTextAsset.text, typeof(MappingKeys));

            return mappingKeys;
        }
    }
}