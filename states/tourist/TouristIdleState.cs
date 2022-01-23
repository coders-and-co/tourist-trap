using Godot;

namespace Duality.states.tourist
{
    public class TouristIdleState : BaseState<Tourist>
    {
        public override void OnEnter()
        {
            GD.Print("Tourist Idle : ", this);
        }
    }
}