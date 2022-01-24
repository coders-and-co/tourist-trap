using System.Threading;
using Godot;

namespace Duality.states.tourist
{
    public class TouristTakePictureState : BaseState<Tourist>
    {
        public override void OnEnter()
        {
            RefObj.CameraSprite.Frame = 0;

            RefObj.CameraSprite.Play("takePicture");
            RefObj.LinearVelocity = Vector2.Zero;
            Area2D play = (Area2D) RefObj.PlayerToFollow;
            RefObj.FeaturesPhotographed.Add(play.GetRid().GetId());
            
        }

        public override BaseState<Tourist> Update(float delta)
        {
            if (!RefObj.CameraSprite.IsPlaying())
            {
                RefObj.PlayerToFollow = null;
                RefObj.FindTarget();
                return new TouristIdleState();   
            }

            return null;
        }
    }
}