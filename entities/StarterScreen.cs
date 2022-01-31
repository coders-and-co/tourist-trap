using Godot;
using System;
using Object = Godot.Object;

public class StarterScreen : Node2D
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey key && key.Pressed)
            QueueFree();
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
            QueueFree();

    }

}
