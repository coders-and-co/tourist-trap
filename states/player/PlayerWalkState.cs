using Godot;

namespace Duality.states.player
{
    public class PlayerWalkState : BaseState<Player>
    {
        public override void OnEnter()
        {
            RefObj.BodySprite.Play("walk");
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
                    RefObj.BodySprite.FlipH = false;
                }
                else
                {
                    RefObj.BodySprite.FlipH = true;
                }
                RefObj.MoveAndSlide(movement.Normalized() * RefObj.Speed);
                return null;
            }
        }
    }
}