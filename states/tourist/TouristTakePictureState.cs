using System.Threading;
using Godot;

namespace Duality.states.tourist
{
    public class TouristTakePictureState : BaseState<Tourist>
    {
        public override void OnEnter()
        {
            RefObj.LinearVelocity = Vector2.Zero;
            RefObj.CameraSprite.Play("takePicture");
            Area2D play = (Area2D) RefObj.PlayerToFollow;
            RefObj.FeaturesPhotographed.Add(play.GetRid().GetId());
            
        }

        public override BaseState<Tourist> Update(float delta)
        {
            RefObj.PlayerToFollow = null;
            RefObj.FindTarget();
            return new TouristIdleState();
        }
    }
}