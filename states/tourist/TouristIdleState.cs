using Godot;

namespace Duality.states.tourist
{
    public class TouristIdleState : BaseState<Tourist>
    {
        private Vector2 _meander = new Vector2(40, 0);
        public override void OnEnter()
        {
            RefObj.BodySprite.Play("walk");
        }

        public override BaseState<Tourist> Update(float delta)
        {
            if (Input.IsActionJustPressed("interact"))
            {
                return new TouristFollowState();
            }
            else
            {
                _meander = _meander.Rotated(GD.Randf() - 0.5f);
                RefObj.LinearVelocity = _meander;
                return null;
            }
        }
    }
}