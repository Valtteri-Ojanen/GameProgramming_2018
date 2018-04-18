using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.Messaging
{
    public class GameEndedMessage: IMessage
    {
        public bool GameWon { get; private set; }
        public bool GameLost { get; private set; }

        /// <summary>
        /// Message that tells if the game has been won or lost
        /// </summary>
        /// <param name="gameWon"> true if game has been won otherwise false </param>
        /// <param name="gameLost"> true if game has been lost otherwise false</param>
        public GameEndedMessage( bool gameWon, bool gameLost )
        {
            GameWon = gameWon;
            GameLost = gameLost;
        }
    }
}
