using Godot;

namespace Duality.states.npc
{
    public class NPCMeanderState : BaseState<NPC>
    {
        public override string GetName() { return "Meander"; }
        private float _timer;
        private Vector2 _force;
        public Vector2 Force { get => _force; }
        public override void OnEnter()
        {
            _timer = (float) GD.RandRange(1, 2); // 1 to 2 seconds
            _force = new Vector2(RefObj.Speed, 0);
            _force = _force.Rotated(GD.Randf() * Mathf.Pi * 2); 
            RefObj.BodySprite.Play("walk");
        }
        
        public override BaseState<NPC>? Update(float delta)
        {
            if (_timer <= 0)
            {
                return new NPCIdleState();
            }
            _force = _force.Rotated(GD.Randf() * 0.2f - 0.1f); // add warble
            _timer -= delta;
            return null;
        }
    }
}