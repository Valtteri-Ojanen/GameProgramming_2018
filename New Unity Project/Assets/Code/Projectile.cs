using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public class Projectile: MonoBehaviour
    {
        [SerializeField]
        private float _damage;

        [SerializeField]
        private float _shootingForce;

        [SerializeField]
        private float _explosionForce;

        [SerializeField]
        private float explosionRadius;

        private Weapon _weapon;
        private Rigidbody _rigidbody;

        private System.Action< Projectile > _collisionCallback;

        public Rigidbody Rigidbody
        {
            get
            {
                if(_rigidbody == null)
                {
                    _rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
                    _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                }
                return _rigidbody;
            }
        }

        public void Init (System.Action< Projectile> collisionCallback)
        {
            _collisionCallback = collisionCallback;
        }

        public void Launch (Vector3 direction)
        {
            //TODO: Add particle effects.
            Rigidbody.AddForce(direction.normalized * _shootingForce, ForceMode.Impulse);
        }

        protected void OnCollisionEnter( Collision collision )
        {
            //TODO: Add particle effects.
            //TODO: Apply Damage to enemies.
            Rigidbody.velocity = Vector3.zero;
            _collisionCallback(this);
        }
    }
}
