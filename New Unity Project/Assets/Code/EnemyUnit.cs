using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankGame.AI;
using System.Linq;
using TankGame.WaypointSystem;

namespace TankGame
{
    public class EnemyUnit: Unit
    {

        [SerializeField]
        private float _detectEnemyDistance;
        [SerializeField]
        private float _shootingDistance;
        [SerializeField]
        private float _arriveDistance;

        [SerializeField]
        private Path _path;

        [SerializeField]
        private Direction _direction;

        private IList<AIStateBase> _states = new List<AIStateBase>();

        public AIStateBase CurrentState { get; private set; }
        public float DetectEnemyDistance { get { return _detectEnemyDistance; } }
        public float ShootingDistance { get { return _shootingDistance; } }
        public PlayerUnit Target { get; set; }

        public Vector3? ToTargetVector
        {
            get
            {
                if(Target != null)
                {
                    return Target.transform.position - transform.position;
                }
                return null;
            }
        }

        public override void Init()
        {
            base.Init();
            InitStates();
        }

        private void InitStates ()
        {
            PatrolState patrol = new PatrolState(this, _path, _direction, _arriveDistance);
            _states.Add(patrol);
            FollowTargetState follow = new FollowTargetState(this);
            _states.Add(follow);
            ShootState shoot = new ShootState(this);
            _states.Add(shoot);
            CurrentState = patrol;
            CurrentState.StateActivated();
        }

        protected override void Update()
        {
            CurrentState.Update();
        }

        public bool PerformTransition( AIStateType targetState)
        {
            if( !CurrentState.CheckTransition(targetState)) {
                return false;
            }

            bool result = false;
            AIStateBase state = GetStateByType(targetState);
            if(state != null)
            {
                CurrentState.StateDeactivating();
                CurrentState = state;
                CurrentState.StateActivated();
                result = true;
            }
            return result;
            
        }

        private AIStateBase GetStateByType ( AIStateType stateType)
        {
            return _states.FirstOrDefault(( state ) => state.State == stateType);
        }

    }
}
