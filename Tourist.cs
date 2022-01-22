using Godot;
using System;

public class Tourist : RigidBody2D
{
    private Node2D sprites;
    private AnimatedSprite bodySprite;
    private AnimatedSprite faceSprite;
    private AnimatedSprite clothesSprite;
    private AnimatedSprite hatSprite;
    
    private Vector2 force = new Vector2(50, 0);
    
    public override void _Ready()
    {
        sprites = GetNode<Node2D>("Sprites");
        bodySprite = GetNode<AnimatedSprite>("Sprites/Body");
        faceSprite = GetNode<AnimatedSprite>("Sprites/Face");
        clothesSprite = GetNode<AnimatedSprite>("Sprites/Clothes");
        hatSprite = GetNode<AnimatedSprite>("Sprites/Hat");
        
        faceSprite.Frame = (int) GD.Randi() % faceSprite.Frames.GetFrameCount("happy");
        clothesSprite.Frame = (int) GD.Randi() % clothesSprite.Frames.GetFrameCount("default");
        hatSprite.Frame = (int) GD.Randi() % hatSprite.Frames.GetFrameCount("default");
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        force = force.Rotated(GD.Randf() - 0.5f);
        LinearVelocity = force;

        if (force.x > 0)
        {
            sprites.Scale = new Vector2(-1, 1);
            // bodySprite.FlipH = true;
            // faceSprite.FlipH = true;
        }
        else
        {
            // bodySprite.FlipH = false;
            // faceSprite.FlipH = false;
            sprites.Scale = new Vector2(1, 1);
        }
    }
}