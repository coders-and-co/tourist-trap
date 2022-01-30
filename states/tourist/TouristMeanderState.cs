using Godot;

namespace Duality.states.tourist
{
    public class TouristMeanderState : BaseState<Tourist>
    {
        public override string GetName() { return "Meander"; }
        private float _timer;
        private Vector2 _force;
        public Vector2 Force
        {
            get => _force;
        }
        public override void OnEnter()
        {
            _timer = (float) GD.RandRange(1, 2); // 1 to 2 seconds
            _force = new Vector2(RefObj.Speed, 0);
            _force = _force.Rotated(GD.Randf() * Mathf.Pi * 2); 
            RefObj.BodySprite.Play("walk");
            RefObj.Vision.Connect("body_entered", this, "Spotted");
            RefObj.Vision.Connect("area_entered", this, "Spotted");
        }

        public void Spotted(Node2D thing) 
        {
            // when we spot a new potential target, instantly check it
            _timer = 0;
        }

        public override BaseState<Tourist>? Update(float delta)
        {
            if (_timer <= 0)
            {
                var (target, score) = RefObj.FindTarget(); 
                if (target != null && score > RefObj.MinFollowScore && target.Position.DistanceTo(RefObj.Position) > RefObj.ComfortDistance)
                    return new TouristFollowState(target, score);
                else
                    return new TouristIdleState();
            }
            _force = _force.Rotated(GD.Randf() * 0.2f - 0.1f); // add warble
            _timer -= delta;
            return null;
        }
    }
}