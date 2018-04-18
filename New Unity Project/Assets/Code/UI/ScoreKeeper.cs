using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using L10n = TankGame.Localization.Localization;

namespace TankGame.UI
{
    public class ScoreKeeper: MonoBehaviour
    {

        private Text _text;

        // Use this for initialization
        private void Start()
        {
            _text = GetComponentInChildren<Text>();
            GameManager.Instance.ScoreChanged += SetText;
            SetText(GameManager.Instance.Score);
        }

        private void OnDestroy()
        {
            GameManager.Instance.ScoreChanged -= SetText;
        }

        private void SetText( float score )
        {
            // Sets score text that is called when Gamemanagers score has changed or language has been changed
            _text.text = string.Format("{0} : {1} ",  L10n.CurrentLanguage.GetTranslation("Score"), score);
        }
    }
}
