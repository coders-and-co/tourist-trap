using Godot;

namespace Duality.states.player
{
    public class PlayerIdleState : BaseState<Player>
    {
        private Vector2? _throwTo = null;
        public override void OnEnter()
        {
            RefObj.BodySprite.Play("idle");
        }

        public override BaseState<Player> Update(float delta)
        {
            Vector2 movement = RefObj.GetMovementVector();

            if (_throwTo.HasValue)
                return new PlayerThrowState(_throwTo.Value);
            else if (movement.LengthSquared() > 0)
                return new PlayerWalkState();
            
            return null;    
            
        }

        public override void OnLeftClick(Vector2 position)
        {
            _throwTo = position;
        }
    }
}