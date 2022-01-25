using Godot;

namespace Duality.states
{
    public class FiniteStateMachine<T>
    {
        // public Game Root;
        private readonly T RefObj;
        public BaseState<T> CurrentState;
        
        public FiniteStateMachine(T refObj, BaseState<T> defaultState)
        {
            GD.Print("Creating FSM for ", refObj);
            RefObj = refObj;
            CurrentState = defaultState;
            CurrentState.RefObj = RefObj;
            CurrentState.OnEnter();
        }

        public void OnLeftClick(Vector2 position)
        {
            CurrentState.OnLeftClick(position);
        }

        public void Update(float delta)
        {
            BaseState<T> nextState = CurrentState.Update(delta);
            if (nextState != null)
            {
                CurrentState.OnExit();
                CurrentState = nextState;
                CurrentState.RefObj = RefObj;
                CurrentState.OnEnter();
            }
        }
    }
}