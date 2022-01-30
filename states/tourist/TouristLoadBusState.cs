using Godot;

namespace Duality.states.tourist
{
    public class TouristLoadBusState : BaseState<Tourist>
    {
        public override string GetName() { return "GET ON BUS"; }
        
        private readonly Node2D _target;
        private float _score;
        private bool _excited;
        private float _speed;
        private Vector2 _force;
        public Vector2 Force { get => _force; }

        public TouristLoadBusState(Node2D target)
        {
            _target = target;
        }

        public override string GetDebugState() { return IsInstanceValid(_target) ? $"{_target.Name}" : ""; } //  ({_score:N0})
        public Node2D? GetDebugTarget() { return _target; }
        
        public override void OnEnter()
        {
         
            RefObj.BodySprite.Play("walk");
        }

      

        public override BaseState<Tourist>? Update(float delta)
        {
            
            
            // calculate delta vector and distance
            Vector2 d = _target.Position - RefObj.Position;
            var dist = d.Length();
            if (dist < 100)
            {
                RefObj.QueueFree();
                Map.TouristCount--;
            }

            // Adjust speed based on target score
            _speed = 50;
            
            // Follow the thing!
            _force = d.Normalized() * _speed;
            return null;
        }
        
    }
}