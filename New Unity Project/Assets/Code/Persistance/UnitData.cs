using System;

namespace TankGame.Persistance
{
    [Serializable]
    public class UnitData
    {
        public int Id;
        public int Health;
        public SerializableVector3 Position;
        public float YRotation;
    }
}
