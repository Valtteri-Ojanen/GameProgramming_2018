using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.AI
{
    public enum AIStateType
    {
        Error = 0,
        Patrol = 1,
        FollowTarget = 2,
        Shoot = 3
    }

    public abstract class AIStateBase
    {
        public AIStateType State { get; protected set; }
        public IList<AIStateType> TargetStates { get; protected set; }
        public EnemyUnit Owner { get; protected set; }

        protected AIStateBase()
        {
            TargetStates = new List<AIStateType>();
        }

        protected AIStateBase( EnemyUnit owner, AIStateType state )
            : this()
        {
            State = state;
            Owner = owner;
        }

        public bool AddTransition (AIStateType targetState)
        {
            return TargetStates.AddUnique(targetState);
        }

        public bool RemoveTransition( AIStateType targetState)
        {
            return TargetStates.Remove(targetState);
        }

        
        public virtual bool CheckTransition(AIStateType targetState)
        {
            return TargetStates.Contains(targetState);
        }

        public virtual void StateActivated()
        {

        }

        public virtual void StateDeactivating()
        {

        }

        public abstract void Update();
    }
}
