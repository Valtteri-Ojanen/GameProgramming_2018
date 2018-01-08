using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public abstract class Unit: MonoBehaviour
    {
        protected IMover _mover;

        [SerializeField]
        public float moveSpeed, turnSpeed; 


        protected IMover mover
        {
            get { return _mover; }
        }

        protected void Awake()
        {
            Init();
        }

        public virtual void Init()
        {
            _mover = gameObject.GetOrAddComponent<TransformMover>();
            //_mover = GetComponent<IMover>();

            mover.Init(moveSpeed, turnSpeed);
        }

        public virtual void Clear()
        {

        }

        protected abstract void Update();
    }
}
