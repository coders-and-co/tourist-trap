using Godot;
using System;

public class Tourist : RigidBody2D
{
    private AnimatedSprite bodySprite;
    private AnimatedSprite faceSprite;
    private Vector2 force = new Vector2(50, 0);
    
    public override void _Ready()
    {
        bodySprite = GetNode<AnimatedSprite>("Sprites/Body");
        faceSprite = GetNode<AnimatedSprite>("Sprites/Face");
        faceSprite.Frame = (int) GD.Randi() % faceSprite.Frames.GetFrameCount("happy");
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        force = force.Rotated(GD.Randf() - 0.5f);
        LinearVelocity = force;

        if (force.x > 0)
        {
            bodySprite.FlipH = true;
            faceSprite.FlipH = true;
        }
        else
        {
            bodySprite.FlipH = false;
            faceSprite.FlipH = false;
        }
    }
}