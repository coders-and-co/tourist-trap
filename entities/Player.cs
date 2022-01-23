using Godot;
using System;
using Duality.states;
using Duality.states.player;

public class Player : KinematicBody2D
{
    // Finite State Machine
    public FiniteStateMachine<Player> FSM;
    
    [Export] public int Speed = 240;
    public AnimatedSprite BodySprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Save Node references
        BodySprite = GetNode<AnimatedSprite>("Body");
        // Create FSM
        FSM = new FiniteStateMachine<Player>(this, new PlayerIdleState());
    }

    public Vector2 GetMovementVector()
    {
        Vector2 movement = Vector2.Zero;
        if (Input.IsActionPressed("move_left"))
        {
            movement.x = -1;
        }
        else if (Input.IsActionPressed("move_right"))
        {
            movement.x = 1;
        }
        if (Input.IsActionPressed("move_up"))
        {
            movement.y = -1;
        }
        else if (Input.IsActionPressed("move_down"))
        {
            movement.y = 1;
        }
        return movement;
    }

    public override void _PhysicsProcess(float delta)
    {

        FSM.Update(delta);
        
    }
}
