using Godot;
namespace Duality.states.player
{
    public class PlayerShoutState : BaseState<Player>
    {
        
        public override void OnEnter()
        {
            RefObj.BodySprite.Play("shout");
            GD.Print("Shout!");
            
        }
        
        public override void OnExit()
        {
            GD.Print("Shout Done!");
        }

        public override BaseState<Player> Update(float delta)
        {
            if (RefObj.BodySprite.IsPlaying() == false)
                return new PlayerIdleState();
            return null;
        }
    }
}