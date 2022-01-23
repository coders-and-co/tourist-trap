using Godot;

namespace Duality.states.tourist
{
    public class TouristFollowState : BaseState<Tourist>
    {
        public override void OnEnter()
        {
            RefObj.LinearVelocity = Vector2.Zero;
            RefObj.BodySprite.Play("idle");
        }

        public override BaseState<Tourist> Update(float delta)
        {
            if (Input.IsActionJustPressed("interact"))
            {
                return new TouristIdleState();
            }
            else
            {
                return null;
            }
        }
    }
}