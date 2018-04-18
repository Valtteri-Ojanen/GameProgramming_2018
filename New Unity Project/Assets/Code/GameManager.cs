using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TankGame.Persistance;
using System.Linq;
using TankGame.Messaging;
using TankGame.Localization;
using System;
using UnityEngine.SceneManagement;

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
        [SerializeField]
        private float _scoreToWin = 1000f;
        [SerializeField]
        private int _playerDeathsToLose = 3;
        private List<Unit> _enemyUnits = new List<Unit>();
        private Unit _playerUnit;
        private SaveSystem _saveSystem;
        private float _score;
        private int _playerDeaths;

        public event Action<float> ScoreChanged;

        public int PlayerDeaths
        {
            get { return _playerDeaths; }
            private set { _playerDeaths = value; }
        }

        public string SavePath
        {
            get { return Path.Combine(Application.persistentDataPath, "save"); }
        }

        public float Score
        {
            get { return _score; }
        }

        public MessageBus MessageBus { get; private set; }

        public void Awake()
        {
            Time.timeScale = 1f;
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

        private void OnDestroy()
        {
            Localization.Localization.LanguageLoaded -= OnLanguageLoaded;
            // Used to prevent making new gamemanagers when scene is reloading.
            IsClosing = true;
        }

        private void Init ()
        {
            InitLocalization();
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

        private const string LanguageKey = "Language";

        private void InitLocalization()
        {
            LangCode currentLang = (LangCode) PlayerPrefs.GetInt(LanguageKey, (int)LangCode.EN);
            Localization.Localization.LoadLanguage(currentLang);
            Localization.Localization.LanguageLoaded += OnLanguageLoaded;
        }

        private void OnLanguageLoaded()
        {
            PlayerPrefs.SetInt(LanguageKey, (int)Localization.Localization.CurrentLanguage.LanguageCode);
            // Refreshes the score text when language is changed
            if(ScoreChanged != null)
            {
                ScoreChanged(_score);
            }
        }

        /// <summary>
        /// Called when player has won the game by obtaining enough score.
        /// </summary>
        private void GameWon()
        {
            MessageBus.Publish(new GameEndedMessage( gameWon : true, gameLost : false));
            Time.timeScale = 0;
        }

        /// <summary>
        /// Called when player has death counter exceeds the limit.
        /// </summary>
        private void GameLost()
        {
            MessageBus.Publish(new GameEndedMessage( gameWon: false, gameLost: true));
            Time.timeScale = 0;
        }

        /// <summary>
        /// Called when player has died, checks if the current amount of playerdeaths is enough to lose the game
        /// </summary>
        public void PlayerDied()
        {
            PlayerDeaths++;
            Debug.Log("playerDied " + PlayerDeaths);
            if(PlayerDeaths >= _playerDeathsToLose)
            {
                GameLost();
            }
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

        /// <summary>
        /// Adds points to cu
        /// </summary>
        /// <param name="amount"></param>
        public void AddScore(float amount)
        {
            _score += amount;
            if(ScoreChanged != null)
            {
                ScoreChanged(_score);
            }
            if(_score >= _scoreToWin)
            {
                GameWon();
            }
        }

        public void Exit()
        {
            Application.Quit();
        }
        
        public void Restart()
        {
            SceneManager.LoadScene("Level1");
        }
    }
}
