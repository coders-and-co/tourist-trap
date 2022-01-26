using Godot;
namespace Duality.states.player
{
    public class PlayerShoutState : BaseState<Player>
    {
        private bool _done;
        
        public override void OnEnter()
        {
            switch (GD.Randi() % 3 + 1)
            {
                case 1:
                    RefObj.Audio.Stream = GD.Load<AudioStream>("res://assets/sfx/Processed sfx/hey_1_p.mp3");
                    break;
                case 2:
                    RefObj.Audio.Stream = GD.Load<AudioStream>("res://assets/sfx/Processed sfx/hey_2_p.mp3");
                    break;
                case 3:
                    RefObj.Audio.Stream = GD.Load<AudioStream>("res://assets/sfx/Processed sfx/hey_3_p.mp3");
                    break;
            }

            RefObj.Audio.Play();
            RefObj.BodySprite.Play("shout");
            RefObj.BodySprite.Connect("animation_finished", this, "Done", null, (uint) ConnectFlags.Oneshot);
        }
        
        public override void OnExit()
        {
            GD.Print("Shout Done!");
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