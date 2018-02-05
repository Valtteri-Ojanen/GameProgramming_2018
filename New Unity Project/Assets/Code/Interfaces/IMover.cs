using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public interface IMover
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moveSpeed"></param>
        /// <param name="turnSpeed"></param>
        void Init( float moveSpeed, float turnSpeed );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        void Move( float amount );

        void Turn( float amount );

        void Move( Vector3 direction );

        void Turn( Vector3 target );

    }
}

