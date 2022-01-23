using Godot;
namespace Duality.states.player
{
    public class PlayerThrowState : BaseState<Player>
    {
        
        private PackedScene _flagScene = GD.Load<PackedScene>("entities/Flag.tscn");
        private Vector2 _throwTo;

        public PlayerThrowState(Vector2 position)
        {
            _throwTo = position;
        }
        public override void OnEnter()
        {
            RefObj.BodySprite.Play("throw");
            GD.Print("Throwing!");
        }
        
        public override void OnExit()
        {
            GD.Print("Throwing Done!");
            Flag flag = _flagScene.Instance<Flag>();
            Root.GetNode("Entities").AddChild(flag);
            flag.Position = _throwTo;
        }

        public override BaseState<Player> Update(float delta)
        {

            if (RefObj.BodySprite.IsPlaying() == false)
                return new PlayerIdleState();
            
            return null;

        }
    }
}