using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.Persistance
{
    public class SaveSystem
    {
        private IPersistance _persistance;

        public SaveSystem(IPersistance persistance)
        {
            _persistance = persistance;
        }

        public void Save (GameData data)
        {
            _persistance.Save(data);
        }

        public GameData Load()
        {
            return _persistance.Load<GameData>();
        }

    }
}
