using System;
using System.Collections.Generic;

namespace TankGame.Persistance
{
    [Serializable]
    public class GameData
    {
        public UnitData PlayerData;
        public List<UnitData> EnemyDatas = new List<UnitData>();
    }
}
