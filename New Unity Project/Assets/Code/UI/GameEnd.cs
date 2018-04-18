using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankGame.Messaging;

namespace TankGame.UI
{
    public class GameEnd: MonoBehaviour
    {
        [SerializeField]
        private GameObject _gameLost;
        [SerializeField]
        private GameObject _gameWon;
        [SerializeField]
        private GameObject _buttons;

        private ISubscription<GameEndedMessage> _gameEndedSubscription;

        // Use this for initialization
        private void Start()
        {
            DisableGameEndUI();

            _gameEndedSubscription = GameManager.Instance.MessageBus.Subscribe<GameEndedMessage>(OnGameEnd);
        }

        /// <summary>
        /// Inspects GameEndedMessage to see if the game has been lost or won
        /// </summary>
        /// <param name="msg">Contains information about the game end state </param>
        private void OnGameEnd(GameEndedMessage msg)
        {
            if(msg.GameWon)
            {
                ShowGameWonUI();
            } else if (msg.GameLost)
            {
                ShowGameLostUI();
            } else
            {
                Debug.Log("There seems to be an error in the messaging system");
            }
        }

        /// <summary>
        /// Displays needed UI for the game won state
        /// </summary>
        private void ShowGameWonUI()
        {
            _gameWon.SetActive(true);
            _buttons.SetActive(true);
        }

        /// <summary>
        /// Displays needed UI for the game Lost state
        /// </summary>
        private void ShowGameLostUI()
        {
            _gameLost.SetActive(true);
            _buttons.SetActive(true);
        }

        /// <summary>
        /// Disables all UI components that are meant only for gameending screen
        /// </summary>
        private void DisableGameEndUI ()
        {
            _gameLost.SetActive(false);
            _gameWon.SetActive(false);
            _buttons.SetActive(false);
        }

        private void OnDestroy()
        {
            GameManager.Instance.MessageBus.UnSubscribe(_gameEndedSubscription);
        }

    }
}
