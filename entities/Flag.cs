using Godot;
using System;

public class Flag : KinematicBody2D
{

    private Vector2? _target;
    
    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_target.HasValue)
        {
            Position = Position.LinearInterpolate(_target.Value, 0.2f);
            var dist = (_target.Value - Position).Length();
            if (dist < 1)
                _target = null;
        }
    }

    public void Throw(Vector2 from, Vector2 to)
    {
        Position = from;
        _target = to;
    }
}
