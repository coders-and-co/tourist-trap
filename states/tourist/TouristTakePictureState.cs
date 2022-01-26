using System.Threading;
using Godot;

namespace Duality.states.tourist
{
	public class TouristTakePictureState : BaseState<Tourist>
	{
		public override string GetName() { return "TakePicture"; }
		private bool _done = false;
		private readonly Node2D _target;
		public TouristTakePictureState(Node2D target)
		{
			_target = target;
		}
		
		public override void OnEnter()
		{
			RefObj.LinearVelocity = Vector2.Zero;
			RefObj.AddPhoto(_target);
			
			RefObj.BodySprite.Play("idle");
			RefObj.CameraSprite.Visible = true;
			RefObj.CameraSprite.Play("take_picture");
			RefObj.CameraSprite.Frame = 0;
			RefObj.CameraSprite.Connect("animation_finished", this, "Done", null, (uint) ConnectFlags.Oneshot);
		}

		public override void OnExit()
		{
			RefObj.CameraSprite.Visible = false;
		}

		public override BaseState<Tourist> Update(float delta)
		{
			if (_done)
				return new TouristIdleState();
			// Wait for camera animation to finish
			return null;
		}
		
		public void Done() { _done = true; }
		
	}
}
