using Godot;

namespace Duality.states.tourist
{
    public class TouristMeanderState : BaseState<Tourist>
    {
        private Vector2 _meander = new Vector2(40, 0);
        public int MeanderCountDown;
        public override void OnEnter()
        {
            RefObj.BodySprite.Play("walk");
            MeanderCountDown = (int) GD.RandRange(100,350);
        }

        public override BaseState<Tourist> Update(float delta)
        {
            if (RefObj.PlayerToFollow != null)
            {
                return new TouristFollowState();
            }
            else if(MeanderCountDown > 0)
            {
                _meander = _meander.Rotated(GD.Randf() - 0.5f);
                MeanderCountDown--;
                RefObj.LinearVelocity = _meander.Normalized()*RefObj.Speed;
                return null;
            }  else if (MeanderCountDown == 0)
            {
                return new TouristIdleState();
            }
            return null;
        }
    }
}