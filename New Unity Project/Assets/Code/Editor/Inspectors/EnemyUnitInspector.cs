using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TankGame.Editor
{
    [CustomEditor(typeof(EnemyUnit))]
    public class EnemyUnitInspector: UnitInspector
    {
        private EnemyUnit _target;
        private int _damageAmount = 10;

        protected override void OnEnable()
        {

            base.OnEnable();
            _target = target as EnemyUnit;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;
            GUILayout.Label("Provide damage to the unit", EditorStyles.boldLabel);

            _damageAmount = EditorGUILayout.IntField("Damage amount", _damageAmount);
            if(GUILayout.Button(string.Format("Take {0} damage", _damageAmount)))
            {
                _target.TakeDamage(_damageAmount);
            }
            GUI.enabled = true;

        }
    }
}
