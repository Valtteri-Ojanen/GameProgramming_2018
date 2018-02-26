using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TankGame.Editor
{
    [CustomEditor(typeof(EnemyUnit))]
    public class EnemyUnitInspector: UnityEditor.Editor
    {
        private EnemyUnit _target;
        private int _damageAmount = 10;

        protected void OnEnable()
        {
            _target = target as EnemyUnit;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;
            GUILayout.Label("Provide damage to the unit");

            _damageAmount = EditorGUILayout.IntField("Damage amount", _damageAmount);
            if(GUILayout.Button(string.Format("Take {0} damage", _damageAmount)))
            {
                _target.TakeDamage(_damageAmount);
            }
            GUI.enabled = true;

        }
    }
}
