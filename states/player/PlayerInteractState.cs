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
            RefObj.BodySprite.Connect("animation_finished", this, "Done", null, (uint) ConnectFlags.Oneshot);
        }
        
        public override void OnExit()
        {
            RefObj.Interact();
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