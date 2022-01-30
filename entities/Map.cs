using Godot;
using System;
using Godot.Collections;

public class Map : Node2D
{
    public static Array<string> TouristsCompletedStatuePhoto = new Array<string>();
    public static int TouristCount = 0;

    public override void _Ready()
    {
        spawn_interest_points();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (TouristsCompletedStatuePhoto.Count == TouristCount)
        {
            GD.Print("before spawn blockers");
            spawn_blockers();
        }
    }

    private void spawn_interest_points()
    {
        PackedScene featureScene = GD.Load<PackedScene>("entities/Feature.tscn");
        PackedScene statueScene = GD.Load<PackedScene>("entities/Statue.tscn");
        Node2D mapBits = GetNode<Node2D>("Objects");
        
         foreach (Node2D obj in mapBits.GetChildren())
         {
             GD.Print(obj.Name);
             Area2D structure = obj.Name switch
             {
                 "Statue" => statueScene.Instance<Area2D>(),
                 "Feature" => featureScene.Instance<Area2D>(),
                 _ => null,
             };
            if(structure == null)
                break;
             structure.Position = obj.Position;
             var entities = GetTree().Root.GetNode<Node2D>("Game/Entities");
             entities.AddChild(structure);
             obj.QueueFree(); //delete blank node2Ds
         }
    }
    
    public void spawn_blockers()
    {
        GD.Print("in spawn blockers");
        PackedScene blockerScene = GD.Load<PackedScene>("entities/Blocker.tscn");
        GD.Print("1");

        Node2D mapBits = GetNode<Node2D>("Objects");
        GD.Print("2");

        foreach (Node2D obj in mapBits.GetChildren())
        {
            Node2D blocker;
            GD.Print(obj.Name, obj);
            if (obj.Name == "Blocker")
            {
                blocker = blockerScene.Instance<Node2D>();
                blocker.Position = obj.Position;
                var entities = GetTree().Root.GetNode<Node2D>("Game/Entities");
                entities.AddChild(blocker);
            }

            obj.QueueFree(); //delete blank node2Ds
        }
    }
    
    public static void CheckStatueWinCondition()
    {
        if (TouristsCompletedStatuePhoto.Count == TouristCount)
        {
            
        }
    }
}
