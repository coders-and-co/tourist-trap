using Godot;

namespace Duality.states.tourist
{
    public class TouristIdleState : BaseState<Tourist>
    {
        public override string GetName() { return "Idle"; }
        private float _timer;
        
        public override void OnEnter()
        {
            _timer = GD.Randf() * 1.0f + 1.0f; // 1 to 2s
            RefObj.LinearVelocity = Vector2.Zero;
            RefObj.BodySprite.Play("idle");
            RefObj.FaceSprite.Play("default");
            RefObj.PickRandomFrame(RefObj.FaceSprite);
        }

        public override void OnExit()
        {
            // RefObj.FaceSprite.Play("default");
            // RefObj.PickRandomFrame(RefObj.FaceSprite);
        }

        public override BaseState<Tourist> Update(float delta)
        {
            if (_timer <= 0)
            {
                // Look for targets at end of idle
                var (target, score) = RefObj.FindTarget(); 
                if (target != null && score > 20)
                    return new TouristFollowState(target);
                else if (GD.Randf() > 0.6667f)
                    return new TouristTalkState();
                else
                    return new TouristMeanderState();
            }
            _timer -= delta;
            return null;
        }
    }
}