using Godot;
using System;

public class Flag : KinematicBody2D
{

    private AnimatedSprite _flagSprite;
    
    public bool Moving = false; 
    private Vector2? _origin;
    private Vector2? _target;
    private float _dist;
    private float _t = 0.0f;
    
    public override void _Ready()
    {
        _flagSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Moving && _target.HasValue && _origin.HasValue)
        {
            // interpolate position along vector
            Position = _origin.Value.LinearInterpolate(_target.Value, _t);
            _t += 0.1f;


            if (_t >= 1)
            {
                Position = _target.Value;
                Moving = false;
            }
        }
    }

    public void Throw(Vector2 from, Vector2 to)
    {
        _origin = Position = from;
        _target = to;
        _dist = (to - from).Length();
        Moving = true;
        _flagSprite.Play("boing");
    }
}
