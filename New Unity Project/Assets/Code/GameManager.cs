﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TankGame.Persistance;
using System.Linq;
using TankGame.Messaging;
using TankGame.Localization;

namespace TankGame
{
    public class GameManager: MonoBehaviour
    {
#region Statics
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if(_instance == null && !IsClosing)
                {
                    GameObject gameManager = new GameObject(typeof(GameManager).Name);
                    _instance = gameManager.AddComponent<GameManager>();
                }
                return _instance;
            }
        }

        public static bool IsClosing { get; private set; }

        #endregion Statics
        private List<Unit> _enemyUnits = new List<Unit>();
        private Unit _playerUnit;
        private SaveSystem _saveSystem;

        public string SavePath
        {
            get { return Path.Combine(Application.persistentDataPath, "save"); }
        }

        public MessageBus MessageBus { get; private set; }

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
        private void OnApplicationQuit()
        {
            IsClosing = true;
        }

        private void Init ()
        {
            Localization.Localization.LoadLanguage(LangCode.EN);
            IsClosing = false;
            MessageBus = new MessageBus();
            var UI = FindObjectOfType<UI.UI>();
            UI.Init();

            Unit[] allUnits = FindObjectsOfType<Unit>();
            foreach(Unit unit in allUnits)
            {
                AddUnit(unit);
            }
            _saveSystem = new SaveSystem(new BinaryPersistance(SavePath));
        }

        protected void Update()
        {
            bool save = Input.GetKeyDown(KeyCode.F2);
            bool load = Input.GetKeyDown(KeyCode.F3);

            if(save)
                Save();
            else if(load)
                Load();
        }

        private void AddUnit( Unit unit )
        {
            unit.Init();

            if(unit is EnemyUnit)
            {
                _enemyUnits.Add(unit);
            }
            else if(unit is PlayerUnit)
            {
                _playerUnit = unit;
            }
            UI.UI.Current.HealthUI.AddUnit(unit);
        }

        public void Save()
        {
            GameData data = new GameData();
            foreach (Unit unit in _enemyUnits)
            {
                data.EnemyDatas.Add(unit.GetUnitData());
            }

            data.PlayerData = _playerUnit.GetUnitData();
            _saveSystem.Save(data);
        }

        public void Load ()
        {
            GameData data = _saveSystem.Load();
            foreach( UnitData enemyData in data.EnemyDatas)
            {
                Unit enemy = _enemyUnits.FirstOrDefault(unit => unit.Id == enemyData.Id);
                if(enemy != null)
                {
                    enemy.SetUnitData(enemyData);
                }
            }
            _playerUnit.SetUnitData(data.PlayerData);
        }
    }
}
