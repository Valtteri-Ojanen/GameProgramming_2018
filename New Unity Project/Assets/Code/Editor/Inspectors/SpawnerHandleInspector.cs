using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;

namespace TankGame.Editor {
    public class SpawnerHandleInspector: UnityEditor.Editor {

        [UnityEditor.CustomEditor(typeof(CollectibleSpawner))]
        public class PinballManagerInspector: UnityEditor.Editor
        {
            private CollectibleSpawner targetPM;
            private Transform handleTransform;
            private Quaternion handleQuaternion;

            protected void OnEnable()
            {
                targetPM = target as CollectibleSpawner;
                handleTransform = targetPM.transform;
            }

            /// <summary>
            /// Display handle for both min and max spawn positions
            /// </summary>
            private void OnSceneGUI()
            {
                // gives reference for the variable itself so it can be edited directly
                // without needing to set it afterwards
                DisplayLaunchPointHandle(ref targetPM.MinSpawnPosition);
                DisplayLaunchPointHandle(ref targetPM.MaxSpawnPosition);
            }

            /// <summary>
            /// Displays handles in the scene and allows them to be moved in editor
            /// </summary>
            /// <param name="targetPoint"> position of the handle currently in the world</param>
            private void DisplayLaunchPointHandle(ref Vector3 targetPoint)
            {
                // Sets the handle's rotation based on
                // if Local or Global mode is active
                handleQuaternion = Tools.pivotRotation == PivotRotation.Local ?
                    handleTransform.rotation : Quaternion.identity;

                // Handle current position in the world
                Vector3 point =
                    targetPoint;

                EditorGUI.BeginChangeCheck();

                point = Handles.DoPositionHandle(point, handleQuaternion);

                if(EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(targetPM, "MoveHandle");
                    targetPoint =
                        new Vector3(point.x,0.5f,point.z);
                }
            }
        }
    }
}
