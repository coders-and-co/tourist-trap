using Godot;
namespace Duality.states.player
{
    public class PlayerShoutState : BaseState<Player>
    {
        private bool _done;
        
        public override void OnEnter()
        {
            GD.Print("Shout!");
            RefObj.BodySprite.Play("shout");
            RefObj.BodySprite.Connect("animation_finished", this, "Done");
        }
        
        public override void OnExit()
        {
            GD.Print("Shout Done!");
        }

        public override BaseState<Player> Update(float delta)
        {
            if (_done)
                return new PlayerIdleState();
            return null;
        }
        
        public void Done() { _done = true; }
    }
}