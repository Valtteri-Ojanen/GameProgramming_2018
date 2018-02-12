using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankGame.Systems;

namespace TankGame.AI
{
    public class FollowTargetState: AIStateBase
    {

        public float SqrShootingDistance
        {
            get { return Owner.ShootingDistance * Owner.ShootingDistance; }
        }

        public float SqrDetectEnemyDistance
        {
            get { return Owner.DetectEnemyDistance * Owner.DetectEnemyDistance; }
        }

        public FollowTargetState( EnemyUnit owner)
            : base(owner, AIStateType.FollowTarget)
        {
            AddTransition(AIStateType.Shoot);
            AddTransition(AIStateType.Patrol);
        }

        public override void StateActivated()
        {
            base.StateActivated();
        }

        public override void Update()
        {
            if(!ChangeState())
            {
                Owner.Mover.Turn(Owner.Target.transform.position);
                Owner.Mover.Move(Owner.transform.forward);
            }
        }

        private bool ChangeState ()
        {
            Vector3 toPlayerVector = Owner.transform.position - Owner.Target.transform.position;
            float sqrDistanceToPlayer = toPlayerVector.sqrMagnitude;
            if(sqrDistanceToPlayer < SqrShootingDistance)
                return Owner.PerformTransition(AIStateType.Shoot);

            if(sqrDistanceToPlayer > SqrDetectEnemyDistance)
            {
                Owner.Target = null;
                return Owner.PerformTransition(AIStateType.Patrol);
            }

            return false;
        }

    }
}