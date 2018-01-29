using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TankGame.AI;
using System.Linq;

namespace TankGame
{
    public class EnemyUnit: Unit
    {
        private IList<AIStateBase> _states = new List<AIStateBase>();

        public AIStateBase CurrentState { get; private set; }

        public override void Init()
        {
            base.Init();
            InitStates();
        }

        private void InitStates ()
        {
            // TODO: Implement me!
        }

        protected override void Update()
        {
            return;
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
