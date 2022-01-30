using Godot;
using System;
using Duality.states;
using Duality.states.npc;
using Duality.states.tourist;
using Priority_Queue;

[Tool]
public class NPC : RigidBody2D, IEntity
{
    
    public enum NPCType
    {
        Normal,
        Sketchy,
        Barista,
    }
    
    public Node2D Sprites;
    public AnimatedSprite BodySprite;
    public AnimatedSprite FaceSprite;
    public AnimatedSprite OutfitSprite;
    public AudioStreamPlayer2D Audio;
    
    // State variables
    
    private NPCType _type;
    public FiniteStateMachine<NPC> StateMachine;
    public int Influence = 0;
    int IEntity.Influence { get => Influence; }
    
    // Tunables
    [Export] public int Speed = 3750;
    [Export] public NPCType Type
    {
        get => _type;
        set => SetType(value);
    }

    public void SetType(NPCType type)
    {
        _type = type;

        Influence = _type switch
        {
            NPCType.Normal => 0,
            NPCType.Sketchy => 60,
            NPCType.Barista => 60,
            _ => 0
        };
        
        if (OutfitSprite is null || FaceSprite is null)
            return;
        
        OutfitSprite.Animation = FaceSprite.Animation = _type switch
        {
            NPCType.Normal => "default",
            NPCType.Sketchy => "sketchy",
            NPCType.Barista => "barista",
            _ => "default"
        };
        
        // randomize NPC outfit
        switch (_type)
        {
            case NPCType.Normal:
                OutfitSprite.Frame = (int) GD.Randi() % 5;
                break;
            case NPCType.Sketchy:
                OutfitSprite.Play();
                break;
            
        }
    }
    
    public override void _Ready()
    {
        // lookup node references
        Sprites = GetNode<Node2D>("Sprites");
        BodySprite = GetNode<AnimatedSprite>("Sprites/Body");
        FaceSprite = GetNode<AnimatedSprite>("Sprites/Face");
        OutfitSprite = GetNode<AnimatedSprite>("Sprites/Outfit");
        Audio = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        
        // create state machine
        StateMachine = new FiniteStateMachine<NPC>(this, new NPCIdleState());
        SetType(_type);
    }
    
    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        switch (StateMachine.CurrentState)
        {
            case NPCMeanderState st:
                LinearDamp = 20;
                AppliedForce = st.Force * 20;
                break;
            default:
                AppliedForce = Vector2.Zero;
                LinearDamp = 2000;
                break;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        // don't animate in the editor
        if (Engine.EditorHint)
            return; 
        
        StateMachine.Update(delta);
        // Flip sprites if moving right
        if (LinearVelocity.x > 5)
            Sprites.Scale = new Vector2(-1, 1);
        else if (LinearVelocity.x < -5)
            Sprites.Scale = new Vector2(1, 1);
    }
    
}
