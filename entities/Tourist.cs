using Godot;
using System;

public class Tourist : RigidBody2D
{
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
    }

    private void PickRandomFrame(AnimatedSprite sprite, string anim="default") {
        sprite.Frame = (int) GD.Randi() % sprite.Frames.GetFrameCount(anim);
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