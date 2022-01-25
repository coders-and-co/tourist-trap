using Godot;

namespace Duality.states.tourist
{
    public class TouristMeanderState : BaseState<Tourist>
    {
        private int _timer;
        private Vector2 _meander;
        public override string GetName() { return "Meander"; }
        public override void OnEnter()
        {
            _timer = (int) GD.RandRange(100, 350);
            _meander = new Vector2(RefObj.Speed, 0);
            RefObj.BodySprite.Play("walk");
            RefObj.LinearVelocity = _meander;
        }

        public override BaseState<Tourist> Update(float delta)
        {
            if (_timer == 0)
            {
                Node2D target = RefObj.FindTarget(); // Look for targets
                if (target != null)
                    return new TouristFollowState(target);
                else
                    return new TouristIdleState();
            }
            _meander = _meander.Rotated(GD.Randf() - 0.5f); // Rotate meander vector
            RefObj.LinearVelocity = _meander; // _meander.Normalized() * RefObj.Speed;
            _timer -= 1;
            return null;
        }
    }
}