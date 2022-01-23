using Godot;
using System;
using Duality.states;
using Duality.states.player;
using Duality.states.tourist;

public class Tourist : RigidBody2D
{
    // Finite State Machine
    public FiniteStateMachine<Tourist> FSM;
    
    // Node References
    private Node2D _sprites;
    private AnimatedSprite _bodySprite;
    private AnimatedSprite _faceSprite;
    private AnimatedSprite _outfitSprite;
    private AnimatedSprite _bodyAccessorySprite;
    private AnimatedSprite _headAccessorySprite;
    
    private Vector2 _force = new Vector2(50, 0);

    public override void _Ready()
    {
        GD.Randomize();
        
        // save node references
        _sprites = GetNode<Node2D>("Sprites");
        _bodySprite = GetNode<AnimatedSprite>("Sprites/Body");
        _faceSprite = GetNode<AnimatedSprite>("Sprites/Face");
        _outfitSprite = GetNode<AnimatedSprite>("Sprites/Outfit");
        _bodyAccessorySprite = GetNode<AnimatedSprite>("Sprites/Body Accessory");
        _headAccessorySprite = GetNode<AnimatedSprite>("Sprites/Head Accessory");

        // randomize tourist
        PickRandomFrame(_faceSprite, "happy");
        PickRandomFrame(_outfitSprite);
        PickRandomFrame(_bodyAccessorySprite);
        PickRandomFrame(_headAccessorySprite);
        
        FSM = new FiniteStateMachine<Tourist>(this, new TouristIdleState());

    }

    private void PickRandomFrame(AnimatedSprite sprite, string anim="default") {
        sprite.Frame = (int) GD.Randi() % sprite.Frames.GetFrameCount(anim);
    }
    
    public void OnStateChanged(string from, string to)
    {
        GD.Print("State Changed!");
        GD.Print(from, "->", to);
    }

    public override void _PhysicsProcess(float delta)
    {
        
        FSM.Update(delta);
        
        if (LinearVelocity.x > 0)
        {
            _sprites.Scale = new Vector2(-1, 1);
        }
        else {
            _sprites.Scale = new Vector2(1, 1);
        }
    }
    
}