using Godot;

namespace Duality.states
{
    public class FiniteStateMachine<T>
    {
        public Game Root;
        public T RefObj;
        public BaseState<T> CurrentState;
        
        public FiniteStateMachine(Game rootNode, T refObj, BaseState<T> defaultState)
        {
            GD.Print("Creating FSM for ", refObj);

            Root = rootNode;
            RefObj = refObj;
            
            CurrentState = defaultState;
            CurrentState.Root = Root;
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
                CurrentState.Root = Root;
                CurrentState.RefObj = RefObj;
                CurrentState.OnEnter();
            }
        }
    }
}