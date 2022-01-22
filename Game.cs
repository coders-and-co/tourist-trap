using Godot;
using System;

public class Game : Node2D
{

    // private Player _player;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Hello from C#!");
        // _player = GetNode<Player>("Player");
    }
    
}
