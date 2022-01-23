using Godot;

namespace Duality.states
{
    public class BaseState<T>
    {
        public T RefObj;
        public Game Root;

        public BaseState()
        {
            RefObj = default(T);
        }

        // public BaseState(T refObj)
        // {
        //     RefObj = refObj;
        // }
        
        public virtual void OnEnter()
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        public virtual void OnLeftClick(Vector2 position)
        {
            
        }

        public virtual BaseState<T> Update(float delta)
        {
            return null;
        }
    }
}