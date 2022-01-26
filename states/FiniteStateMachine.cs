using Godot;

namespace Duality.states
{
    public class FiniteStateMachine<T>
    {
        // public Game Root;
        private readonly T _refObj;
        public BaseState<T>? CurrentState;
        
        public FiniteStateMachine(T refObj, BaseState<T> defaultState)
        {
            GD.Print("Creating FSM for ", refObj);
            _refObj = refObj;
            ChangeState(defaultState);
        }

        public void ChangeState(BaseState<T> nextState)
        {
            CurrentState?.OnExit();
            CurrentState = nextState;
            CurrentState.RefObj = _refObj;
            CurrentState.OnEnter();
        }

        public void Update(float delta)
        {
            var nextState = CurrentState!.Update(delta);
            if (nextState != null)
                ChangeState(nextState);
        }
        
        public void OnLeftClick(Vector2 position)
        {
            CurrentState?.OnLeftClick(position);
        }
    }
}