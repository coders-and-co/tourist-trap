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
        PackedScene statueScene = GD.Load<PackedScene>("entities/Statue.tscn");
        Node2D mapBits = GetNode<Node2D>("Objects");
        
         foreach (Node2D obj in mapBits.GetChildren())
         {
             Area2D structure = obj.Name switch
             {
                 "Statue" => statueScene.Instance<Area2D>(),
                 _ => featureScene.Instance<Area2D>()
             };

             structure.Position = obj.Position;
             var entities = GetTree().Root.GetNode<Node2D>("Game/Entities");
             entities.AddChild(structure);
             obj.QueueFree(); //delete blank node2Ds
         }
    }
}
