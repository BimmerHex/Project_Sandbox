using UnityEngine;

namespace Game.Patterns.StateMachine
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State CurrentState;

        private void Update()
        {
            CurrentState?.Update();
        }

        private void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }

        public void SwitchState(State newState)
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();
        }
    }
}