using Godot;

namespace Duality.states.tourist
{
    public class TouristIdleState : BaseState<Tourist>
    {
        private int _timer;
        public override string GetName() { return "Idle"; }
        
        public override void OnEnter()
        {
            _timer = (int) GD.RandRange(50, 250);
            RefObj.BodySprite.Play("idle");
            RefObj.FaceSprite.Play("talk");
            RefObj.LinearVelocity = Vector2.Zero;
        }

        public override void OnExit()
        {
            RefObj.FaceSprite.Play("default");
            RefObj.PickRandomFrame(RefObj.FaceSprite);
        }

        public override BaseState<Tourist> Update(float delta)
        {
            if (_timer == 0)
            {
                Node2D target = RefObj.FindTarget(); // Look for targets
                if (target != null)
                    return new TouristFollowState(target);
                else
                    return new TouristMeanderState();
            }
            _timer -= 1;
            return null;
        }
    }
}