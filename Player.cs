using Godot;
using System;

public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        
        var d = new Vector2();
        
        if (Input.IsActionPressed("ui_left"))
        {
            d.x = -1;
        }
        else if (Input.IsActionPressed("ui_right"))
        {
            d.x = 1;
        }
        if (Input.IsActionPressed("ui_up"))
        {
            d.y = -1;
        }
        else if (Input.IsActionPressed("ui_down"))
        {
            d.y = 1;
        }

        Position += d.Normalized() * 5;
        
    }
    
}
