using UnityEngine;
using System;

namespace TankGame.Persistance
{
    [Serializable]
    public class SerializableVector3: MonoBehaviour
    {
        public float X;
        public float Y;
        public float Z;

        public SerializableVector3 (float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public SerializableVector3(Vector3 vector)
            : this(vector.x, vector.y, vector.z)
        {
        }
        
        public SerializableVector3(Vector2 vector)
            : this(vector.x,vector.x,0)
        {

        }
       
    }
}
