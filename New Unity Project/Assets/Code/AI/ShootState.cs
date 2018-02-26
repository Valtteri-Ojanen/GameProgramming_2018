using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.AI {
public class ShootState : AIStateBase {

        /// <summary>
        /// Sqr distance calculated from Owners shooting distance
        /// </summary>
        public float SqrShootingDistance
        {
            get { return Owner.ShootingDistance * Owner.ShootingDistance; }
        }

        /// <summary>
        /// Constructor for shootstate, adds 2 states that are allowed to transition to from this state.
        /// </summary>
        /// <param name="owner">Enemyunit that uses this class</param>
        public ShootState( EnemyUnit owner )
            : base(owner, AIStateType.Shoot)
        {
            AddTransition(AIStateType.FollowTarget);
            AddTransition(AIStateType.Patrol);
        }

        /// <summary>
        /// Uses baseclass implementation of activating this state.
        /// </summary>
        public override void StateActivated()
        {
            base.StateActivated();
        }


        /// <summary>
        /// Checks if the state should be changed, if not turn towards player and shoot.
        /// </summary>
        public override void Update()
        {
            if(!ChangeState())
            {
                Owner.Mover.Turn(Owner.Target.transform.position);
                Owner.Weapon.Shoot();
            }
        }

        /// <summary>
        /// Checks if the player is within given shooting distance, if not change state to follow player. If player has died, switch back to patrolling state.
        /// </summary>
        /// <returns> True if changing state was needed and the transition was succesful, false if should continue using this state</returns>
        private bool ChangeState()
        {
            Vector3 toPlayerVector = Owner.transform.position - Owner.Target.transform.position;
            float sqrDistanceToPlayer = toPlayerVector.sqrMagnitude;
            if(sqrDistanceToPlayer > SqrShootingDistance)
                return Owner.PerformTransition(AIStateType.FollowTarget);
            else if(!Owner.Target.gameObject.activeInHierarchy)
            {
                Owner.Target = null;
                return Owner.PerformTransition(AIStateType.Patrol);
            }
            

            return false;
        }

    }
}
