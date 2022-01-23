using Godot;

namespace Duality.states.player
{
    public class PlayerIdleState : BaseState<Player>
    {
        public override void OnEnter()
        {
            RefObj._bodySprite.Play("idle");
        }

        public override BaseState<Player> Update(float delta)
        {
            Vector2 movement = RefObj.GetMovementVector();
            if (movement.LengthSquared() > 0)
            {
                return new PlayerWalkState();
            }
            else
            {
                return null;    
            }
        }
    }
}