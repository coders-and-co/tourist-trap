using Godot;
using System;
using Godot.Collections;

public class Map : Node2D
{
    public static Array<string> TouristsCompletedStatuePhoto = new Array<string>();
    public static int TouristCount = 0;
    public static bool BusTakeMeHome = false;

    public override void _Ready()
    {
        spawn_interest_points();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (TouristsCompletedStatuePhoto.Count == TouristCount && !BusTakeMeHome)
        {
            spawn_blockers();
            var traffic = GetTree().Root.GetNode<Traffic>("Game/Entities/Cars/TrafficFlow");
            traffic.Toggle(false);
        }
        if (BusTakeMeHome && TouristCount == 0)
        {
            GD.Print("WONNNNN");
            Game.Win = true;
        }
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
        BusTakeMeHome = true;
        PackedScene blockerScene = GD.Load<PackedScene>("entities/Blocker.tscn");

        Node2D mapBits = GetNode<Node2D>("Objects");

        foreach (Node2D obj in mapBits.GetChildren())
        {
            Node2D blocker;
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
    
}
