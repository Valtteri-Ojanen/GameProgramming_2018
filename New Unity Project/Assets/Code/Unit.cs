using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public abstract class Unit: MonoBehaviour
    {

        public virtual void Init()
        {

        }

        public virtual void Clear()
        {

        }

        protected abstract void Update();
    }
}
