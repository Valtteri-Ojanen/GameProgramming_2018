using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankGame.WaypointSystem;
using TankGame.Systems;

namespace TankGame.AI
{
    public class PatrolState: AIStateBase
    {
        private Path _path;
        private Direction _direction;
        private float _arriveDistance;
        public Waypoint CurrentWaypoint { get; private set; }

        private float _honkDistance;
        private AudioSource _honkSound;
        private bool _honkUsed = false;

        public PatrolState (EnemyUnit owner, Path path, Direction direction, float arriveDistance, float honkDistance, AudioSource honkSound)
        {
            State = AIStateType.Patrol;
            Owner = owner;
            AddTransition(AIStateType.FollowTarget);
            _path = path;
            _direction = direction;
            _arriveDistance = arriveDistance;
            _honkDistance = honkDistance;
            _honkSound = honkSound;
        }

        public override void StateActivated()
        {
            base.StateActivated();
            CurrentWaypoint = _path.GetClosestWaypoint(Owner.transform.position);
        }

        public override void Update()
        {
            // 1. should we change the state?
            // 1.1 if yes, change state and return
            if(!ChangeState())
            {
                int mask = 1 << LayerMask.NameToLayer("Enemy");
                Collider[] enemies = Physics.OverlapSphere(Owner.transform.position, _honkDistance, mask);
                if(enemies.Length > 2 && !_honkUsed)
                {
                    _honkSound.Play();
                    _honkUsed = true;
                } else if (enemies.Length == 2)
                {
                    _honkUsed = false;
                }
                CurrentWaypoint = GetWaypoint();
                // 2. Are we close enough the current waypoint
                // 2.1 if yes, get next waypoint
                Owner.Mover.Turn(CurrentWaypoint.Position);
                Owner.Mover.Move(Owner.transform.forward);

                // 3. Move towards current waypoint
                // 4. Rotate towards current waypoint
            }

        }

        private Waypoint GetWaypoint ()
        {
            Waypoint result = CurrentWaypoint;

            Vector3 toWaypointVector = CurrentWaypoint.Position - Owner.transform.position;
            float toWaypointSqr = toWaypointVector.sqrMagnitude;
            float sqrArriveDistance = _arriveDistance * _arriveDistance;
            if(toWaypointSqr <= sqrArriveDistance)
            {
                result = _path.GetNextWaypoint(CurrentWaypoint, ref _direction);
            }

            return result;
        }

        private bool ChangeState()
        {
            int playerLayer = LayerMask.NameToLayer("Player");
            int mask = Flags.CreateMask(playerLayer);
            Collider[] players = Physics.OverlapSphere(Owner.transform.position, Owner.DetectEnemyDistance, mask);
            if (players.Length > 0)
            {
                PlayerUnit player = players[0].gameObject.GetComponentInHierarchy<PlayerUnit>();
                if(player != null)
                {
                    Owner.Target = player;
                    float sqrDistanceToPlayer = Owner.ToTargetVector.Value.sqrMagnitude;
                    if( sqrDistanceToPlayer < Owner.DetectEnemyDistance * Owner.DetectEnemyDistance)
                    {
                        return Owner.PerformTransition(AIStateType.FollowTarget);
                    }

                    Owner.Target = null;
                }
            }
            return false;
        }
    }
}
