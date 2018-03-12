using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankGame.Persistance;

namespace TankGame.Testing
{
    public class OperatorTesting: MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            var first = new Vector3(1, 2, 3);
            var second = new SerializableVector3(3, 2, 1);

            var result = first + second;
            Debug.Log(result);
        }
    }
}
