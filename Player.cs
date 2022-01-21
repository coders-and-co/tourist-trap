using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    public int Speed = 5;
    private Vector2? _target = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_target.HasValue)
        {
            var d = (Vector2) _target - Position;
            if (d.Length() < Speed)
            {
                Position = (Vector2) _target;
                _target = null;
            }
            else
            {
                Position += d.Normalized() * Speed;    
            }
        }
    }
    
    public void SetTarget(Vector2 t)
    {
        _target = t;
    }
    
}
