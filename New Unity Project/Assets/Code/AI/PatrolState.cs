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

        public PatrolState (EnemyUnit owner, Path path, Direction direction, float arriveDistance)
        {
            State = AIStateType.Patrol;
            Owner = owner;
            AddTransition(AIStateType.FollowTarget);
            _path = path;
            _direction = direction;
            _arriveDistance = arriveDistance;
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
                Owner.Target = player;
                Owner.PerformTransition(AIStateType.FollowTarget);
                return true;
            }
            return false;
        }
    }
}
