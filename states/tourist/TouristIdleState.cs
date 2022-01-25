using Godot;

namespace Duality.states.tourist
{
    public class TouristIdleState : BaseState<Tourist>
    {
        private float _timer;
        public override string GetName() { return "Idle"; }
        
        public override void OnEnter()
        {
            _timer = GD.Randf() * 3.0f + 1.0f; // 1 to 4s
            RefObj.BodySprite.Play("idle");
            switch (GD.Randi() % 2 + 1)
            {
                case 1:
                    RefObj.FaceSprite.Play("talk");
                    break;
                case 2:
                    RefObj.FaceSprite.Play("default");
                    break;
            }
            
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
                Node2D target = RefObj.FindTarget(); // Look for targets
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