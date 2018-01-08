using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public class TransformMover: MonoBehaviour, IMover
    {
        protected float moveSpeed, turnSpeed;

        public void Init( float moveSpeed, float turnSpeed )
        {
            this.moveSpeed = moveSpeed;
            this.turnSpeed = turnSpeed;
        }

        public void Turn(float amount)
        {
            Vector3 rotation = transform.localEulerAngles;
            rotation.y += amount * turnSpeed * Time.deltaTime;
            transform.localEulerAngles = rotation;
        }

        public void Move (float amount)
        {
            Vector3 position = transform.position;
            Vector3 movement = transform.forward * amount * moveSpeed * Time.deltaTime;
            position += movement;
            transform.position = position;
        }
    }
}