using UnityEditor;
using UnityEngine;

namespace EditorTools.Editor
{
    public static class EditorTools
    {
        [MenuItem("Editor Commands/Play Game %j")]
        private static void PlayGame()
        {
            EditorApplication.isPlaying = true;
            Debug.Log("Playing game!");
        }
    }
}
