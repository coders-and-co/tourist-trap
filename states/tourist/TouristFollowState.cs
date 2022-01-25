using Godot;

namespace Duality.states.tourist
{
    public class TouristFollowState : BaseState<Tourist>
    {
        private float _timer;
        private Node2D _target = null;
        private bool _pointing;
        public override string GetName() { return "Follow"; }
        public TouristFollowState(Node2D target)
        {
            _target = target;
        }
        
        public override void OnEnter()
        {
            _timer = 1.0f;
            RefObj.LinearVelocity = Vector2.Zero;
            RefObj.BodySprite.Play("walk");
            RefObj.PointSprite.Visible = true;
            _pointing = _target.IsInGroup("Feature") || _target.IsInGroup("Bus");
            if (_pointing)
            {
                RefObj.FaceSprite.Play("excite");
                RefObj.PointSprite.Visible = true;
            }
            else
            {
                RefObj.PointSprite.Visible = false;
            }
        }

        public override void OnExit()
        {
            RefObj.FaceSprite.Play("default");
            RefObj.PointSprite.Visible = false;
        }

        public override BaseState<Tourist> Update(float delta)
        {
            // Look for targets
            if(_timer <= 0 || _target == null) {
                var t = RefObj.FindTarget(); 
                if (t == null)
                    return new TouristIdleState(); // Lost target
                if (t != _target)
                    return new TouristFollowState(t);
                _timer = 1.0f;
            }
            
            if (_pointing)
            {
                Vector2 d = RefObj.Position - _target.Position;
                if (RefObj.Sprites.Scale.x < 0)
                    RefObj.PointSprite.Rotation = d.Reflect(Vector2.Up).Angle() - Mathf.Pi / 4;
                else
                    RefObj.PointSprite.Rotation = d.Angle() - Mathf.Pi / 4;
            }
            
            
            var dist = RefObj.Position.DistanceTo(_target.Position);
            // Reached the target
            if (dist <= 192 && _target.IsInGroup("Bus"))
                return new TouristTakePictureState(_target); // Take a pic of target
            else if (dist <= 64 && _target.IsInGroup("Feature"))
                return new TouristTakePictureState(_target); // Take a pic of target
            else if (dist <= 64)
                return new TouristIdleState();

            // Follow the thing!
            _timer -= delta;
            Vector2 direction = _target.Position - RefObj.Position;
            RefObj.LinearVelocity = direction.Normalized() * RefObj.Speed * 1.5f;
            return null;
        }
    }
}