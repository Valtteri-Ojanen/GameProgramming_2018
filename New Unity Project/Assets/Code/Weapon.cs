﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public class Weapon: MonoBehaviour
    {
        [SerializeField]
        private Projectile _projectilePrefab;

        [Tooltip("Ammo / Second")]
        [SerializeField]
        private float _firingRate = 1 / 3f;

        [SerializeField]
        private Transform _shootingPoint;

        private Pool<Projectile> _projectiles;
        private Unit _owner;
        private bool _canShoot = true;
        private float _firingTimer = 0;

        public void Init (Unit owner)
        {
            _owner = owner;
            _projectiles = new Pool<Projectile>(4, false, _projectilePrefab, item => item.Init(this));
        }

        public bool Shoot ()
        {
            if(!_canShoot)
            {
                return false;
            }

            Projectile projectile = _projectiles.GetPooledObject();
            if(projectile != null)
            {
                projectile.transform.position = _shootingPoint.position;
                projectile.Launch(transform.forward);
                _canShoot = false;
            }

            return projectile != null;
        }

        protected virtual void Update()
        {
            UpdateFiringTimer();
        }

        private void UpdateFiringTimer()
        {
            if(!_canShoot)
            {
                _firingTimer += Time.deltaTime;
                if(_firingTimer >= _firingRate)
                {
                    _canShoot = true;
                    _firingTimer = 0;
                }
            }
        }

        public void ProjectileHit (Projectile projectile)
        {
            if(!_projectiles.ReturnObject(projectile))
            {
                Debug.LogError("Could not return projectile back to the pool! ");
            }
        }
    }
}
