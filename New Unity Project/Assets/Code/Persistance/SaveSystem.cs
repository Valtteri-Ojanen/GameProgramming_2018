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

    }
}
