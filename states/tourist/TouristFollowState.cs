using Godot;

namespace Duality.states.tourist
{
    public class TouristFollowState : BaseState<Tourist>
    {
        public override string GetName() { return "Follow"; }
        
        private float _timer;
        private Node2D _target = null;
        private bool _excited;
        private float _speed;
        private Vector2 _lastPos;

        public TouristFollowState(Node2D target)
        {
            _target = target;
        }

        public override string GetDebugState() { return _target.Name; }
        public Node2D GetDebugTarget() { return _target; }
        
        public override void OnEnter()
        {
            _timer = RefObj.FollowPollingInterval;
            _speed = RefObj.SpeedFollow;
            _excited = _target.IsInGroup("Feature") || _target.IsInGroup("Bus");

            if (!_excited)
            {
                RefObj.LinearVelocity = Vector2.Zero;
                RefObj.BodySprite.Play("walk");
            }
            else
            {
                _speed = RefObj.SpeedFollowExcited;
                RefObj.PointSprite.Visible = true;
                RefObj.FaceSprite.Play("excite");
                switch (GD.Randi() % 3 + 1)
                {
                    case 1:
                        RefObj.FaceSprite.Frame = 0;
                        RefObj.Audio.Stream = GD.Load<AudioStream>("res://assets/sfx/Processed sfx/ooo_1_p.mp3");
                        break;
                    case 2:
                        RefObj.FaceSprite.Frame = 1;
                        RefObj.Audio.Stream = GD.Load<AudioStream>("res://assets/sfx/Processed sfx/ooo_2_p.mp3");
                        break;
                    case 3:
                        RefObj.FaceSprite.Frame = 2;
                        RefObj.Audio.Stream = GD.Load<AudioStream>("res://assets/sfx/Processed sfx/ooo_3_p.mp3");
                        break;
                }
                RefObj.Audio.Play();
            }
        }

        public override void OnExit()
        {
            RefObj.FaceSprite.Play("default");
            RefObj.PointSprite.Visible = false;
        }

        public override BaseState<Tourist> Update(float delta)
        {
            // Check if the target has been disposed (such as the flag)
            if (!IsInstanceValid(_target))
                _target = null;
            
            // Poll for targets every so often
            if(_timer <= 0 || _target == null) {
                var (t, score) = RefObj.FindTarget();
                if (t == null || score < 20)
                    return new TouristIdleState(); // Lost target
                if (t != _target)
                    return new TouristFollowState(t); // New target
                _timer = RefObj.FollowPollingInterval;
            }
            
            // float actualSpeed = (RefObj.Position - _lastPos).Length();
            // _lastPos = RefObj.Position;
            // if (actualSpeed < 1)
            // {
            //     return new TouristIdleState();
            // }
            
            // Calculate delta vector and distance
            Vector2 d = _target.Position - RefObj.Position;
            var dist = d.Length();
            
            // Check if reached the target
            if (dist <= 192 && _target.IsInGroup("Bus"))
                return new TouristTakePictureState(_target); // Take a pic of bus
            else if (dist <= 128 && _target.IsInGroup("Feature"))
                return new TouristTakePictureState(_target); // Take a pic of feature
            else if (dist <= 64)
                return new TouristIdleState();
            
            // Point towards target if excited
            if (_excited)
            {
                var opp = d * -1;
                if (RefObj.Sprites.Scale.x < 0)
                    RefObj.PointSprite.Rotation = opp.Reflect(Vector2.Up).Angle() - Mathf.Pi / 4;
                else
                    RefObj.PointSprite.Rotation = opp.Angle() - Mathf.Pi / 4;
            }
            
            // Follow the thing!
            _timer -= delta;
            RefObj.LinearVelocity = d.Normalized() * _speed;
            return null;
        }
    }
}