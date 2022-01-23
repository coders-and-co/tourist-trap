using Godot;

namespace Duality.states.player
{
    public class PlayerWalkState : BaseState<Player>
    {
        
        private Vector2? _throwTo = null;
        
        public override void OnEnter()
        {
            RefObj.BodySprite.Play("walk");
        }

        public override BaseState<Player> Update(float delta)
        {
            Vector2 movement = RefObj.GetMovementVector();
            
            if (_throwTo.HasValue)
                return new PlayerThrowState(_throwTo.Value);
            else if (movement.LengthSquared() == 0)
                return new PlayerIdleState();
            
            SetPlayerFacing(movement);
            RefObj.MoveAndSlide(movement.Normalized() * RefObj.Speed);
            return null;
        }
        
        public override void OnLeftClick(Vector2 position)
        {
            _throwTo = position;
        }

        public void SetPlayerFacing(Vector2 movement)
        {
            if (movement.x < 0)
            {
                RefObj.Sprites.Scale = Game.ScaleVectorNormal;
            }
            else
            {
                RefObj.Sprites.Scale = Game.ScaleVectorFlipped;
            }
        }
    }
}