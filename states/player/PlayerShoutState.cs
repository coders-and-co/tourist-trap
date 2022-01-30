using Godot;
namespace Duality.states.player
{
    public class PlayerShoutState : BaseState<Player>
    {
        private bool _done;
        
        public override void OnEnter()
        {
            RefObj.Audio.Stream = (GD.Randi() % 3 + 1) switch
            {
                1 => GD.Load<AudioStream>("res://assets/sfx/Processed sfx/hey_1_p.mp3"),
                2 => GD.Load<AudioStream>("res://assets/sfx/Processed sfx/hey_2_p.mp3"),
                3 => GD.Load<AudioStream>("res://assets/sfx/Processed sfx/hey_3_p.mp3"),
            };
            
            RefObj.Audio.Play();
            RefObj.BodySprite.Play("shout");
            RefObj.BodySprite.Connect("animation_finished", this, "Done", null, (uint) ConnectFlags.Oneshot);
            RefObj.Shout();
        }
        
        public override void OnExit()
        {
            
        }

        public override BaseState<Player>? Update(float delta)
        {
            if (_done)
                return new PlayerIdleState();
            return null;
        }
        
        public void Done() { _done = true; }
    }
}