using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TankGame.Localization;
using TankGame.Messaging;

namespace TankGame.UI
{
    public class PlayerDeaths: MonoBehaviour
    {

        private Text _text;

        private ISubscription<UnitDiedMessage> _unitDiedSubscription;

        // Use this for initialization
        void Start()
        {
            _text = GetComponent<Text>();
            SetText();
            Localization.Localization.LanguageLoaded += SetText;
            _unitDiedSubscription = GameManager.Instance.MessageBus.Subscribe<UnitDiedMessage>(PlayerDied);
        }

        /// <summary>
        /// Unregisters from all evenlisteners to avoid unwanted results when the game is restarted.
        /// </summary>
        private void UnregisterEventListeners()
        {
            Localization.Localization.LanguageLoaded -= SetText;
            GameManager.Instance.MessageBus.UnSubscribe(_unitDiedSubscription);
        }

        private void SetText()
        {
            _text.text = string.Format("{0} : {1}",  Localization.Localization.CurrentLanguage.GetTranslation("PlayerDeaths"), GameManager.Instance.PlayerDeaths);
        }

        /// <summary>
        /// Triggered when an UnitDiedMessage is sent to messagebus, it is then inspected
        /// if the unit was PlayerUnit refreshes the UI
        /// </summary>
        /// <param name="msg"></param>
        private void PlayerDied( UnitDiedMessage msg )
        {
            if(msg.DeadUnit is PlayerUnit)
            {
                GameManager.Instance.PlayerDied();
                SetText();
            }
        }

        protected void OnDestroy()
        {
            UnregisterEventListeners();
        }

    }
}
