using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public class Collectible: MonoBehaviour
    {
        [SerializeField]
        private float _points;
        [SerializeField]
        private float _rotationSpeed;

        private System.Action<Collectible> _collisionCallback;
        // Update is called once per frame
        void Update()
        {
            Rotate();
        }

        /// <summary>
        /// Initializes the collectible so it has reference for collectible spawners action
        /// </summary>
        /// <param name="collisionCallback"> Action reference that is used when
        /// this collectible is collected</param>
        public void Init( System.Action<Collectible> collisionCallback )
        {
            _collisionCallback = collisionCallback;
        }

        /// <summary>
        /// Rotates the collectible around y axis
        /// </summary>
        private void Rotate()
        {
            transform.Rotate(Vector3.up * Time.deltaTime * _rotationSpeed);
        }

        /// <summary>
        /// When colliding with player trigger collisioncallback action
        /// so the collectible is returned to the pool
        /// </summary>
        /// <param name="other"> Colliding objects collider </param>
        private void OnTriggerEnter( Collider other )
        {
            GameManager.Instance.AddScore(_points);
            _collisionCallback(this);
        }
    }
}
