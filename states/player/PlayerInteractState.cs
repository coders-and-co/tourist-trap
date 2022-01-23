using Godot;
namespace Duality.states.player
{
    public class PlayerInteractState : BaseState<Player>
    {
        
        public override void OnEnter()
        {
            RefObj.BodySprite.Play("interact");
            GD.Print("Interact!");
        }
        
        public override void OnExit()
        {
            GD.Print("Interact Done!");
        }

        public override BaseState<Player> Update(float delta)
        {
            if (RefObj.BodySprite.IsPlaying() == false)
                return new PlayerIdleState();
            return null;
        }
    }
}