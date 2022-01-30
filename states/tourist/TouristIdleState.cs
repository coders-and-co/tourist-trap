using Godot;

namespace Duality.states.tourist
{
    public class TouristIdleState : BaseState<Tourist>
    {
        public override string GetName() { return "Idle"; }
        private float _timer;
        
        public override void OnEnter()
        {
            _timer = (float) GD.RandRange(1, 2); // 1 to 2 seconds
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

        public override BaseState<Tourist>? Update(float delta)
        {
            if (_timer <= 0)
            {
                // Look for targets at end of idle
                var (target, score) = RefObj.FindTarget();
                if (target != null && Map.BusTakeMeHome && target.Name == "Bus")
                    return new TouristLoadBusState(target);
                if (target != null && score > RefObj.MinFollowScore && target.Position.DistanceTo(RefObj.Position) > RefObj.ComfortDistance && target.Name != "Bus")
                    return new TouristFollowState(target, score);
                else if (GD.Randf() > 0.75)
                    return new TouristTalkState();
                else
                    return new TouristMeanderState();
            }
            _timer -= delta;
            return null;
        }
    }
}