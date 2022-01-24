using Godot;

namespace Duality.states.tourist
{
    public class TouristFollowState : BaseState<Tourist>
    {
        public override void OnEnter()
        {
            RefObj.LinearVelocity = Vector2.Zero;
            RefObj.BodySprite.Play("walk");
        }

        public override BaseState<Tourist> Update(float delta)
        {
            if (RefObj.PlayerToFollow != null)
            {
                RefObj.PlayerToFollow = RefObj.FindTarget();
                if ((RefObj.PlayerToFollow.Name == "Feature") &&
                    RefObj.PlayerToFollow.Position.DistanceTo(RefObj.Position) < 10)
                {
                    return new TouristTakePictureState();
                }

                Vector2 direction = RefObj.PlayerToFollow.Position - RefObj.Position;
                RefObj.LinearVelocity = direction.Normalized()*RefObj.Speed;
            }
            else
            {
                // If there is no PlayerToFollow set, check to see if there are any others in the vicinity before going idle
                RefObj.PlayerToFollow = RefObj.FindTarget();
                return new TouristIdleState();
            }

            return null;

        }
    }
}