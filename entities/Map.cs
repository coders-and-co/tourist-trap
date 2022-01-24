using Godot;
using System;

public class Map : Node2D
{

    public override void _Ready()
    {
        spawn_interest_points();
    }

    public override void _PhysicsProcess(float delta)
    {

    }

    private void spawn_interest_points()
    {
        PackedScene featureScene = GD.Load<PackedScene>("entities/Feature.tscn");
        Node2D mapBits = GetNode<Node2D>("Objects");
        
         foreach (Node2D obj in mapBits.GetChildren())
         { 
             Area2D feature = featureScene.Instance<Area2D>();
             feature.Position = obj.Position;
             var entities = GetTree().Root.GetNode<Node2D>("Game/Entities");
             entities.AddChild(feature);
             obj.QueueFree(); //delete blank node2Ds
         }
    }
}
