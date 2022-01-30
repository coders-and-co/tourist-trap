using Duality.states.tourist;
using Godot;

namespace Duality.states.npc
{
    public class NPCIdleState : BaseState<NPC>
    {
        public override string GetName() { return "Idle"; }
        private float _timer;
        
        public override void OnEnter()
        {
            _timer = (float) GD.RandRange(1, 2); // 1 to 2 seconds
            RefObj.LinearVelocity = Vector2.Zero;
            RefObj.BodySprite.Play("idle");

            if (RefObj.Type == NPC.NPCType.Sketchy) 
                if (GD.Randf() > 0.5)
                   RefObj.FaceSprite.Play("sketchy_laugh");
                else
                   RefObj.FaceSprite.Play("sketchy");
        }

        public override void OnExit()
        {
            if (RefObj.Type == NPC.NPCType.Sketchy) 
                RefObj.FaceSprite.Play("sketchy");
        }

        public override BaseState<NPC>? Update(float delta)
        {
            if (_timer <= 0)
            {
                return new NPCMeanderState();
            }
            _timer -= delta;
            return null;
        }
    }
    
}