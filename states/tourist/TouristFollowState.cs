using Godot;

namespace Duality.states.tourist
{
    public class TouristFollowState : BaseState<Tourist>
    {
        public Player playerToFollow;
        public override void OnEnter()
        {
            RefObj.LinearVelocity = Vector2.Zero;
            RefObj.BodySprite.Play("idle");
        }

        public override BaseState<Tourist> Update(float delta)
        {
            if (RefObj.PlayerToFollow != null)
            {
                
                GD.Print("position", RefObj.PlayerToFollow);
                //RefObj.Position = RefObj.Position.LinearInterpolate(RefObj.PlayerToFollow.Position, );
                Vector2 direction = RefObj.PlayerToFollow.Position - RefObj.Position;
                RefObj.LinearVelocity = direction.Normalized()*RefObj.Speed;
            }
            else
            {
                return new TouristIdleState();
            }

            //RefObj.Position = RefObj.Position.LinearInterpolate(RefObj.GlobalPosition, RefObj.PlayerToFollow, delta * 10);
            return null;
            // if (Input.IsActionJustPressed("interact"))
            // {
            //     return new TouristIdleState();
            // }
            // else
            // {
            //     return null;
            // }
        }
    }
}