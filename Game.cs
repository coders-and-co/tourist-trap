using Godot;
using System;

public class Game : Node2D
{

    private Player _player;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Hello from C#!");
        _player = GetNode<Player>("Player");
    }
    
    public override void _Input(InputEvent @event)
    {
        // Mouse in viewport coordinates.
        if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.ButtonIndex == (int) ButtonList.Left && eventMouseButton.Pressed)
            {
                GD.Print("Mouse Click at: ", eventMouseButton.Position);
                _player.SetTarget(eventMouseButton.Position);
            }
        }
    }
    
}
