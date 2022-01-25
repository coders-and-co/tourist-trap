using Godot;

namespace Duality.states.tourist
{
    public class TouristIdleState : BaseState<Tourist>
    {
        private float _timer;
        public override string GetName() { return "Idle"; }
        
        public override void OnEnter()
        {
            _timer = GD.Randf() * 2.0f + 1.0f; // 1 to 3s
            // Randomly talk or smile
            switch (GD.Randi() % 2 + 1)
            {
                case 1:
                    RefObj.FaceSprite.Play("talk");
                    break;
                case 2:
                    RefObj.FaceSprite.Play("default");
                    break;
            }
            RefObj.BodySprite.Play("idle");
            RefObj.LinearVelocity = Vector2.Zero;
        }

        public override void OnExit()
        {
            RefObj.FaceSprite.Play("default");
            RefObj.PickRandomFrame(RefObj.FaceSprite);
        }

        public override BaseState<Tourist> Update(float delta)
        {
            if (_timer <= 0)
            {
                // Look for targets at end of idle
                Node2D target = RefObj.FindTarget(); 
                if (target != null)
                    return new TouristFollowState(target);
                else
                    return new TouristMeanderState();
            }
            _timer -= delta;
            return null;
        }
    }
}