using Godot;

namespace Duality.states
{
    public class BaseState<T> : Godot.Object
    {
        public T RefObj;
        public virtual string GetName() { return "BaseState"; }
        
        public BaseState()
        {
            RefObj = default(T);
        }
        
        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void OnLeftClick(Vector2 position) { }

        public virtual BaseState<T> Update(float delta) {
            return null;
        }
    }
}