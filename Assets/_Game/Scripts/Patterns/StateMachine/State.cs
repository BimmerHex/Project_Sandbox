using UnityEngine;

namespace Game.Patterns.StateMachine
{
    public abstract class State
    {
        protected abstract void OnEnter();
        protected abstract void OnUpdate();
        protected abstract void OnFixedUpdate();
        protected abstract void OnExit();

        public void Enter() => OnEnter();
        public void Update() => OnUpdate();
        public void FixedUpdate() => OnFixedUpdate();
        public void Exit() => OnExit();
    }
}