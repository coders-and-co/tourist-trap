using Godot;

namespace Duality.states.tourist
{
    public class TouristFollowState : BaseState<Tourist>
    {
        public Player playerToFollow;
        public override void OnEnter()
        {
            RefObj.LinearVelocity = Vector2.Zero;
            RefObj.BodySprite.Play("walk");
        }

        public override BaseState<Tourist> Update(float delta)
        {
            //GD.Print("follow state", RefObj.PlayerToFollow);
            if (RefObj.PlayerToFollow != null)
            {
                
                //GD.Print("position", RefObj.PlayerToFollow);
                Vector2 direction = RefObj.PlayerToFollow.Position - RefObj.Position;
                RefObj.LinearVelocity = direction.Normalized()*RefObj.Speed;
            }
            else
            {
                return new TouristIdleState();
            }

            return null;

        }
    }
}