using System;
using UnityEditor;
using UnityEngine;
using ZonkaZombies.Characters.Enemy.EnemyIA.General;

namespace ZonkaZombies.Characters.Enemy.EnemyIA.Editor
{
    [CustomEditor(typeof(GenericEnemy))]
    public class GenericEnemyInspector : UnityEditor.Editor
    {
        private readonly Type[] _generalTypes =
        {
            typeof(GeneralLoseSight),
            typeof(GeneralPursuitBehavior),
            typeof(GeneralSleepingBehavior),
            typeof(GeneralWalkingBehavior),
            typeof(GeneralIdleBehavior),
            typeof(GeneralAttackBehavior)
        };

        public override void OnInspectorGUI()
        {
            GenericEnemy genericEnemy = (GenericEnemy) target;

            DrawDefaultInspector();

            GUILayout.Space(30);

            GUILayout.BeginVertical("Editor Helpers", GUI.skin.window);

            if (GUILayout.Button("Generate Behaviors", GUI.skin.button))
            {
                GenerateGeneralTypes(genericEnemy);
            }

            if (GUILayout.Button("Implement Missing Behaviors", GUI.skin.button))
            {
                ImplementMissingGeneralTypes(genericEnemy);
            }

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.MaxHeight(0);
        }

        private void GenerateGeneralTypes(GenericEnemy genericEnemy)
        {
            GameObject go = genericEnemy.gameObject;

            var behaviorsToDelete = genericEnemy.gameObject.GetComponents<BaseEnemyBehavior>();

            foreach (BaseEnemyBehavior enemyBehavior in behaviorsToDelete)
            {
                DestroyImmediate(enemyBehavior);
            }

            foreach (Type type in _generalTypes)
            {
                Component component = go.AddComponent(type);

                MonoBehaviour mn = component as MonoBehaviour;

                if (mn)
                {
                    mn.enabled = false;
                }
            }
        }

        private void ImplementMissingGeneralTypes(GenericEnemy genericEnemy)
        {
            foreach (Type type in _generalTypes)
            {
                var behavior = genericEnemy.gameObject.GetComponent(type);
                if (!behavior)
                {
                    genericEnemy.gameObject.AddComponent(type);
                }
            }
        }
    }
}