using Godot;
namespace Duality.states.player
{
    public class PlayerThrowState : BaseState<Player>
    {
        private Vector2 _throwTo;

        public PlayerThrowState(Vector2 position)
        {
            _throwTo = position;
        }
        public override void OnEnter()
        {
            RefObj.BodySprite.Play("throw");
            RefObj.FlagSprite.Visible = false; // Hide flag sprite on Player
            // RefObj.ThrowFlag(_throwTo);
        }
        
        public override void OnExit()
        {
            RefObj.ThrowFlag(_throwTo);
            // GD.Print("Throwing Done!");
        }

        public override BaseState<Player> Update(float delta)
        {
            if (RefObj.BodySprite.IsPlaying() == false)
                return new PlayerIdleState();
            
            return null;
        }
    }
}