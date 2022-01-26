using Godot;

namespace Duality.states.tourist
{
	public class TouristTalkState : BaseState<Tourist>
	{
		public override string GetName() { return "Talk"; }
		private bool _done = false;
		
		public override void OnEnter()
		{
			RefObj.BodySprite.Play("idle");
			RefObj.LinearVelocity = Vector2.Zero;
			switch (GD.Randi() % 6 + 1)
			{
				case 1:
					RefObj.FaceSprite.Play("talk");
					RefObj.Audio.Stream = GD.Load<AudioStream>("res://assets/sfx/Processed sfx/talk_p.mp3");
					break;
				case 2:
					RefObj.FaceSprite.Play("talk 2");
					RefObj.Audio.Stream = GD.Load<AudioStream>("res://assets/sfx/Processed sfx/talk_2_p.mp3");
					break;
				case 3:
					RefObj.FaceSprite.Play("talk 3");
					RefObj.Audio.Stream = GD.Load<AudioStream>("res://assets/sfx/Processed sfx/talk_3_p.mp3");
					break;
				case 4:
					RefObj.FaceSprite.Play("talk 4");
					RefObj.Audio.Stream = GD.Load<AudioStream>("res://assets/sfx/Processed sfx/talk_4_p.mp3");
					break;
				case 5:
					RefObj.FaceSprite.Play("talk 5");
					RefObj.Audio.Stream = GD.Load<AudioStream>("res://assets/sfx/Processed sfx/talk_5_p.mp3");
					break;
				case 6:
					RefObj.FaceSprite.Play("talk 6");
					RefObj.Audio.Stream = GD.Load<AudioStream>("res://assets/sfx/Processed sfx/talk_6_p.mp3");
					break;
			}
			RefObj.Audio.Play();
			RefObj.Audio.VolumeDb = -12;
			RefObj.FaceSprite.Frame = 0;
			RefObj.FaceSprite.Connect("animation_finished", this, "Done", null, (uint) ConnectFlags.Oneshot);
		}

		public override void OnExit()
		{
			RefObj.Audio.VolumeDb = 0;
		}

		public override BaseState<Tourist> Update(float delta)
		{
			if (_done)
				if (GD.Randf() > 0.5f)
					return new TouristIdleState();
				else 
					return new TouristTalkState();
			
			return null;
		}
		
		public void Done() { _done = true; }
	}
}
