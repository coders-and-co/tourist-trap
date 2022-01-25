using Godot;
namespace Duality.states.player
{
    public class PlayerThrowState : BaseState<Player>
    {
        private bool _done;
        private Vector2 _throwTo;

        public PlayerThrowState(Vector2 position)
        {
            _throwTo = position;
        }
        public override void OnEnter()
        {
            RefObj.FlagSprite.Visible = false; // Hide flag sprite on Player
            RefObj.BodySprite.Play("throw");
            RefObj.BodySprite.Connect("animation_finished", this, "Done", null, (uint) ConnectFlags.Oneshot);
        }
        
        public override void OnExit()
        {
            RefObj.ThrowFlag(_throwTo);
            // GD.Print("Throwing Done!");
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