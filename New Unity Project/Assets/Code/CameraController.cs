using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public class CameraController: MonoBehaviour, ICameraFollow
    {
        /// <summary>
        /// Editor clamped float value used to calculate angle.
        /// </summary>
        [SerializeField, Range(5,85)]
        private float _angle;

        /// <summary>
        /// Editor clamped float value used to calculate distance from player.
        /// </summary>
        [SerializeField, Range(2,20)]
        private float _distance;

        [SerializeField]
        private Transform _targetTransform;

        public void SetAngle( float angle )
        {
            _angle = angle;
        }

        public void SetDistance( float distance )
        {
            _distance = distance;
        }

        public void SetTarget( Transform targetTransform )
        {
            _targetTransform = targetTransform;
        }

        /// <summary>
        /// Calculates the angle and distance from player.
        /// </summary>
        void LateUpdate()
        {
            float angle = Mathf.Deg2Rad * _angle;
            // Calculates A from C * Angle
            float y = Mathf.Sin(angle) * _distance;
            // Calculate B from A^2 * C ^ 2
            float x = Mathf.Sqrt(_distance * _distance - y * y);
            Vector3 offset = new Vector3(0, y, 0);
            transform.position = _targetTransform.position + -_targetTransform.forward*x + offset;
            Rotate(_targetTransform.position);
        }

        /// <summary>
        /// Rotates the camera according to player position.
        /// </summary>
        /// <param name="target"> Player position </param>
        public void Rotate( Vector3 target )
        {
            Vector3 direction = target - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 90 * Time.deltaTime);
        }
    }
}
