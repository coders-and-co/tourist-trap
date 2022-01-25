using Godot;
namespace Duality.states.player
{
    public class PlayerInteractState : BaseState<Player>
    {
        private bool _done;
        
        public override void OnEnter()
        {
            GD.Print("Interact!");
            RefObj.BodySprite.Play("interact");
            RefObj.BodySprite.Connect("animation_finished", this, "Done");
        }
        
        public override void OnExit()
        {
            GD.Print("Interact Done!");
        }

        public override BaseState<Player> Update(float delta)
        {
            if (_done)
                return new PlayerIdleState();
            // Wait for interact animation to finish
            return null;
        }
        
        public void Done() { _done = true; }
    }
}