﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankGame.WaypointSystem;
namespace TankGame.AI
{
    public class PatrolState: AIStateBase
    {
        private Path _path;
        private Direction _direction;
        private float _arriveDistance;

        public Waypoint CurrentWaypoint { get; private set; }

        public PatrolState (Unit owner, Path path, Direction direction, float arriveDistance)
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
            
        }
    }
}
