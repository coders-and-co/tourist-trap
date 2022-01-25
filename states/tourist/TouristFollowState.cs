using Godot;

namespace Duality.states.tourist
{
    public class TouristFollowState : BaseState<Tourist>
    {
        private Node2D _target = null;
        public override string GetName() { return "Follow"; }
        public TouristFollowState(Node2D target)
        {
            _target = target;
        }
        
        public override void OnEnter()
        {
            RefObj.LinearVelocity = Vector2.Zero;
            RefObj.BodySprite.Play("walk");
        }

        public override BaseState<Tourist> Update(float delta)
        {
            // Look for targets
            _target = RefObj.FindTarget(); 
            if (_target == null)
                return new TouristIdleState(); // Lost target
            
            var dist = _target.Position.DistanceTo(RefObj.Position);
            
            // Reached the target
            if (dist <= 192 && _target.IsInGroup("Bus"))
                return new TouristTakePictureState(_target); // Take a pic of target
            else if (dist <= 64 && _target.IsInGroup("Feature"))
                return new TouristTakePictureState(_target); // Take a pic of target
            else if (dist <= 64)
                return new TouristIdleState();

            // Follow the thing!
            Vector2 direction = _target.Position - RefObj.Position;
            direction = direction.Rotated(GD.Randf() * 0.2f - 0.1f); // add warble
            RefObj.LinearVelocity = direction.Normalized() * RefObj.Speed * 1.5f;
            return null;
        }
    }
}