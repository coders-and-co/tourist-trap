using Godot;

namespace Duality.states.tourist
{
    public class TouristIdleState : BaseState<Tourist>
    {
        private Vector2 standStill = new Vector2(0, 0);
        public int IdleCountDown;
        public override void OnEnter()
        {
            RefObj.BodySprite.Play("idle");
            IdleCountDown = (int) GD.RandRange(50,250);
        }

        public override BaseState<Tourist> Update(float delta)
        {
            if (RefObj.PlayerToFollow != null)
            {
                return new TouristFollowState();
            }
            else if(IdleCountDown > 0)
            {
                IdleCountDown--;
                RefObj.LinearVelocity = standStill;
                return null;
            } else if (IdleCountDown == 0)
            {
                return new TouristMeanderState();
            }

            return null;
        }
    }
}