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

        public void Move( Vector3 direction )
        {
            direction = direction.normalized;
            Vector3 position = transform.position + direction * moveSpeed * Time.deltaTime;
            transform.position = position;
        }

        public void Turn( Vector3 target )
        {
            Vector3 direction = target - transform.position;
            //direction.y = transform.position.y;
            //direction = direction.normalized;
            //float turnSpeedRad = Mathf.Deg2Rad * turnSpeed * Time.deltaTime;
            //Vector3 rotation = Vector3.RotateTowards(transform.forward, direction,turnSpeedRad,0);
            //transform.rotation = Quaternion.LookRotation(rotation, transform.up);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
}