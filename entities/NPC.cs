using Godot;
using System;
using Duality.states;
using Duality.states.npc;
using Duality.states.tourist;
using Priority_Queue;

public class NPC : RigidBody2D
{
    
    public Node2D Sprites;
    public AnimatedSprite BodySprite;
    public AnimatedSprite FaceSprite;
    public AudioStreamPlayer2D Audio;
    
    // State variables
    public FiniteStateMachine<NPC> StateMachine;
    
    // Tunables
    [Export] public int Speed = 75;
    

    public override void _Ready()
    {
        // lookup node references
        Sprites = GetNode<Node2D>("Sprites");
        BodySprite = GetNode<AnimatedSprite>("Sprites/Body");
        FaceSprite = GetNode<AnimatedSprite>("Sprites/Face");
        Audio = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

        // randomize NPC outfit
        // PickRandomFrame(GetNode<AnimatedSprite>("Sprites/Outfit"));
        GetNode<AnimatedSprite>("Sprites/Outfit").Frame = (int) GD.Randi() % 5;

        // create state machine
        StateMachine = new FiniteStateMachine<NPC>(this, new NPCIdleState());
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
        StateMachine.Update(delta);
        // Flip sprites if moving right
        if (LinearVelocity.x > 5)
            Sprites.Scale = new Vector2(-1, 1);
        else if (LinearVelocity.x < -5)
            Sprites.Scale = new Vector2(1, 1);
    }
    
}
