using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] public int Speed = 4;
    private AnimatedSprite bodySprite;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bodySprite = GetNode<AnimatedSprite>("Body");
        GD.Print(bodySprite);
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 vel = Vector2.Zero;
        if (Input.IsActionPressed("ui_left"))
        {
            vel.x = -1;
            bodySprite.FlipH = false;
        }
        else if (Input.IsActionPressed("ui_right"))
        {
            vel.x = 1;
            bodySprite.FlipH = true;
        }

        if (Input.IsActionPressed("ui_up"))
        {
            vel.y = -1;
        }
        else if (Input.IsActionPressed("ui_down"))
        {
            vel.y = 1;
        }

        if (vel.LengthSquared() == 0)
        {
            bodySprite.Play("idle");
        }
        else
        {
            bodySprite.Play("walk");
        }

        MoveAndCollide(vel.Normalized() * Speed);

    }
}
