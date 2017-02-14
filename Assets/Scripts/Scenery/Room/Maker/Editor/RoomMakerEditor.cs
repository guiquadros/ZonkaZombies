#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Scenery.Room.Maker.Editor
{
    [CustomEditor(typeof(RoomMaker))]
    public class RoomMakerEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Color defaultColor = GUI.color;

            RoomMaker script = (RoomMaker) target;

            GUILayout.Space(10);
            GUILayout.Label("Comandos");

            //TODO Adicionar botões DESTRUIR e GERAR

            GUILayout.BeginHorizontal();

            GUI.color = Color.red;

            if (GUILayout.Button("Destruir", GUILayout.MinHeight(30)))
            {
                script.DestroyExistingRoom();
                EditorUtility.SetDirty(target);
            }

            GUI.color = Color.green;

            if (GUILayout.Button("Gerar", GUILayout.MinHeight(30)))
            {
                script.GenerateRoom();
                EditorUtility.SetDirty(target);
            }

            GUILayout.EndHorizontal();

            GUI.color = defaultColor;

            if (GUILayout.Button("Adicionar paredes", GUILayout.MinHeight(30)))
            {
                script.GenerateWalls();
            }

            if (GUILayout.Button("Adicionar Colisor", GUILayout.MinHeight(30)))
            {
                script.AddColider();
            }

            GUILayout.Space(10);
            GUILayout.Label("Alterar posição do cômodo");

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.color = Color.green * .9f;

            if (GUILayout.Button("-X", GUILayout.MinHeight(30), GUILayout.MinWidth(30)))
            {
                script.gameObject.transform.position += new Vector3(-1,0,0);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.color = Color.blue * .9f;

            if (GUILayout.Button("-Z", GUILayout.MinHeight(30), GUILayout.MinWidth(30)))
            {
                script.gameObject.transform.position += new Vector3(0, 0, -1);
            }

            GUILayout.Space(30);

            if (GUILayout.Button("+Z", GUILayout.MinHeight(30), GUILayout.MinWidth(30)))
            {
                script.gameObject.transform.position += new Vector3(0, 0, +1);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.color = Color.green * .9f;

            if (GUILayout.Button("+X", GUILayout.MinHeight(30), GUILayout.MinWidth(30)))
            {
                script.gameObject.transform.position += new Vector3(+1, 0, 0);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

    }
}

#endif