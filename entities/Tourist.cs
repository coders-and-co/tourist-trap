using Godot;
using System;

public class Tourist : RigidBody2D
{
    private Node2D _sprites;
    private AnimatedSprite _bodySprite;
    private AnimatedSprite _faceSprite;
    private AnimatedSprite _clothesSprite;
    private AnimatedSprite _hatSprite;
    private Vector2 _force = new Vector2(50, 0);
    
    public override void _Ready()
    {
        _sprites = GetNode<Node2D>("Sprites");
        _bodySprite = GetNode<AnimatedSprite>("Sprites/Body");
        _faceSprite = GetNode<AnimatedSprite>("Sprites/Face");
        _clothesSprite = GetNode<AnimatedSprite>("Sprites/Clothes");
        _hatSprite = GetNode<AnimatedSprite>("Sprites/Hat");
        
        _faceSprite.Frame = (int) GD.Randi() % _faceSprite.Frames.GetFrameCount("happy");
        _clothesSprite.Frame = (int) GD.Randi() % _clothesSprite.Frames.GetFrameCount("default");
        _hatSprite.Frame = (int) GD.Randi() % _hatSprite.Frames.GetFrameCount("default");
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        _force = _force.Rotated(GD.Randf() - 0.5f);
        LinearVelocity = _force;

        if (_force.x > 0)
        {
            _sprites.Scale = new Vector2(-1, 1);
            // bodySprite.FlipH = true;
            // faceSprite.FlipH = true;
        }
        else
        {
            // bodySprite.FlipH = false;
            // faceSprite.FlipH = false;
            _sprites.Scale = new Vector2(1, 1);
        }
    }
}