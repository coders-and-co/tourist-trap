using Godot;

namespace Duality.states.tourist
{
    public class TouristFollowState : BaseState<Tourist>
    {
        public override string GetName() { return "Follow"; }
        
        private float _timer;
        private readonly Node2D _target;
        private float _score;
        private bool _excited;
        private float _speed;
        private Vector2 _force;
        public Vector2 Force { get => _force; }

        public TouristFollowState(Node2D target, float score)
        {
            _target = target;
            _score = score;
        }

        public override string GetDebugState() { return IsInstanceValid(_target) ? $"{_target.Name}" : ""; } //  ({_score:N0})
        public Node2D? GetDebugTarget() { return _target; }
        
        public override void OnEnter()
        {
            _timer = RefObj.FollowPollingInterval;
            _excited = _target switch
            {
                NPC {Type: NPC.NPCType.Barista} => true,
                NPC {Type: NPC.NPCType.Sketchy} => true,
                var f when f.IsInGroup("Feature") => true,
                var s when s.IsInGroup("Statue") => true,
                _ => false
            };

            if (_excited)
            {
                RefObj.FaceSprite.Play("excite");
                RefObj.PointSprite.Visible = true;
                RefObj.FaceSprite.Frame = (int) GD.Randi() % 3; // 0 to 2 
                RefObj.Audio.Stream = RefObj.FaceSprite.Frame switch
                {
                    0 => GD.Load<AudioStream>("res://assets/sfx/Processed sfx/ooo_1_p.mp3"),
                    1 => GD.Load<AudioStream>("res://assets/sfx/Processed sfx/ooo_2_p.mp3"),
                    2 => GD.Load<AudioStream>("res://assets/sfx/Processed sfx/ooo_3_p.mp3"),
                    _ => null
                };
                RefObj.Audio.Play();
            }
            
            RefObj.BodySprite.Play("walk");
        }

        public override void OnExit()
        {
            RefObj.FaceSprite.Play("default");
            RefObj.PointSprite.Visible = false;
        }

        public override BaseState<Tourist>? Update(float delta)
        {
            // check if the target has been disposed (such as the flag)
            if (!IsInstanceValid(_target))
                return new TouristIdleState();
            
            // scan for targets when timer expires
            if(_timer <= 0) {
                var (t, score) = RefObj.FindTarget();
                // check for lost target
                if (t is null || score == 0)
                    return new TouristIdleState();
                // check for new target
                if (t != _target && t.Name != "Bus")
                    return new TouristFollowState(t, score);
                _score = score; // update score
                _timer = RefObj.FollowPollingInterval; // reset timer
            }
            
            // calculate delta vector and distance
            Vector2 d = _target.Position - RefObj.Position;
            var dist = d.Length();
            
            // check if reached the target
            switch (_target)
            {
                case var f when f.IsInGroup("Feature") && dist <= RefObj.ComfortDistance:
                    return new TouristTakePictureState(f);
                case var s when s.IsInGroup("Statue") && dist <= RefObj.ComfortDistance:
                    return new TouristTakePictureState(s);
                case NPC n when dist < RefObj.ComfortDistance / 2:
                    return new TouristIdleState();
                case var t when dist <= RefObj.ComfortDistance / 2 && _score < RefObj.MaxStopFollowScore:
                    return new TouristIdleState();    
            }
            
            // Point towards target if excited
            if (_excited)
                PointAt(d);
            
            // Adjust speed based on target score
            _speed = _score switch
            {
                var s when s >= 100 => RefObj.SpeedFollowExcited * 1.5f,
                var s when s >= 50 || _excited => RefObj.SpeedFollowExcited,
                var s when s >= 25 => RefObj.SpeedFollow,
                _ => RefObj.Speed,
            };
            
            // Follow the thing!
            _timer -= delta;
            _force = d.Normalized() * _speed;
            return null;
        }

        private void PointAt(Vector2 d)
        {
            var opp = d * -1;
            if (RefObj.Sprites.Scale.x < 0)
                RefObj.PointSprite.Rotation = opp.Reflect(Vector2.Up).Angle() - Mathf.Pi / 4;
            else
                RefObj.PointSprite.Rotation = opp.Angle() - Mathf.Pi / 4;
        }
    }
}