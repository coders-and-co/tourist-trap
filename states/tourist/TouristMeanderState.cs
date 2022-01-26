using Godot;

namespace Duality.states.tourist
{
    public class TouristMeanderState : BaseState<Tourist>
    {
        private float _timer;
        private Vector2 _meander;
        public override string GetName() { return "Meander"; }
        public override void OnEnter()
        {
            _timer = GD.Randf() * 2.0f + 1.0f; // 1 to 3s
            _meander = new Vector2(RefObj.Speed, 0);
            _meander = _meander.Rotated(GD.Randf() * Mathf.Pi * 2); 
            RefObj.BodySprite.Play("walk");
            RefObj.Vision.Connect("body_entered", this, "Spotted");
            RefObj.Vision.Connect("area_entered", this, "Spotted");
        }

        public void Spotted(Node2D thing) // Area2D or PhysicsBody2D
        {
            _timer = 0;
        }

        public override BaseState<Tourist> Update(float delta)
        {
            if (_timer <= 0)
            {
                Node2D target = RefObj.FindTarget(); // Look for targets
                if (target != null)
                    return new TouristFollowState(target);
                else
                    return new TouristIdleState();
            }
            
            _meander = _meander.Rotated(GD.Randf() * 0.2f - 0.1f); // add warble
            RefObj.LinearVelocity = _meander;
            _timer -= delta;
            return null;
        }
    }
}