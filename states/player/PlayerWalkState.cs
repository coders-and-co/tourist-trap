using Godot;

namespace Duality.states.player
{
    public class PlayerWalkState : BaseState<Player>
    {
        public override void OnEnter()
        {
            RefObj._bodySprite.Play("walk");
        }

        public override BaseState<Player> Update(float delta)
        {
            Vector2 movement = RefObj.GetMovementVector();
            if (movement.LengthSquared() == 0)
            {
                return new PlayerIdleState();
            }
            else
            {
                if (movement.x < 0)
                {
                    RefObj._bodySprite.FlipH = false;
                }
                else
                {
                    RefObj._bodySprite.FlipH = true;
                }
                RefObj.MoveAndSlide(movement.Normalized() * RefObj.Speed);
                return null;
            }
        }
    }
}