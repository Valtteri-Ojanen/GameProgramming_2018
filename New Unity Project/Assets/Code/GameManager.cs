using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public class GameManager: MonoBehaviour
    {
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    GameObject gameManager = new GameObject(typeof(GameManager).Name);
                    _instance = gameManager.AddComponent<GameManager>();
                }
                return _instance;
            }
        }

        private List<Unit> _enemyUnits = new List<Unit>();
        private Unit _playerUnit;

        public void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Init();
        }

        private void Init ()
        {
            Unit[] allUnits = FindObjectsOfType<Unit>();
            foreach(Unit unit in allUnits)
            {
                AddUnit(unit);
            }
        }

        private void AddUnit( Unit unit )
        {
            if(unit is EnemyUnit)
            {
                EnemyUnit enemy = unit as EnemyUnit;
                _enemyUnits.Add(unit);
            }
            else if(unit is PlayerUnit)
            {
                _playerUnit = unit;
            }
        }
    }
}
