using Godot;
using System;
using System.Linq;
using Godot.Collections;

[Tool]
public class Traffic : Node2D
{
    public enum TrafficDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    public string[] Colors = {
        "blue", "red", "black", "white"
    };
    
    private TrafficDirection _direction = TrafficDirection.Right;
    private int _numberCarsAhead = 5;
    private int _numberCarsBehind = 5;
    private Array<AnimatedSprite> _cars = new Array<AnimatedSprite>();
    // private Vector2 _respawnBoundary;
    
    [Export]
    private TrafficDirection Direction
    {
        get => _direction;
        set => SetDirection(value);
    }
    public void SetDirection(TrafficDirection dir)
    {
        _direction = dir;
        ClearCars();
        SpawnCars();
    }
    [Export (PropertyHint.Range, "1,10,")] public int NumberCarsAhead {
        get => _numberCarsAhead;
        set => SetNumberCarsAhead(value);
    }
    [Export (PropertyHint.Range, "1,10,")] public int NumberCarsBehind {
        get => _numberCarsBehind;
        set => SetNumberCarsBehind(value);
    }
    public void SetNumberCarsAhead(int num)
    {
        _numberCarsAhead = num;
        SpawnCars();
    }
    public void SetNumberCarsBehind(int num)
    {
        _numberCarsBehind = num;
        SpawnCars();
    }

    public Vector2 GetTrafficVector()
    {
        switch (Direction)
        {
            case TrafficDirection.Left:
                return Vector2.Left;
            case TrafficDirection.Right:
                return Vector2.Right;
            case TrafficDirection.Up:
                return Vector2.Up;
            case TrafficDirection.Down:
                return Vector2.Down;
            default:
                return Vector2.Zero;
        }
    }

    public Vector2 GetRespawnPoint()
    {
        return GetTrafficVector() * _numberCarsBehind * -384;
    }
    
    public Vector2 GetDespawnPoint()
    {
        return GetTrafficVector() * _numberCarsAhead * 384;
    }
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // if (Engine.EditorHint)
        SpawnCars();
    }

    public void ClearCars()
    {
        // Remove Existing cars
        foreach (Node2D car in _cars)
        {
            GD.Print("Removing car: ", car);
            car.QueueFree();
        }
        _cars.Clear();
    }

    public void SpawnCars()
    {
        Vector2 spawnPos = GetRespawnPoint();
        int totalCars = _numberCarsBehind + _numberCarsAhead;
        
        ClearCars();
        
        // Add new cars        
        foreach (int value in Enumerable.Range(1, totalCars))
        {
            SpawnCar(spawnPos);
            spawnPos += GetTrafficVector() * 384;
        }
    }

    public void SpawnCar(Vector2 position = default)
    {
        PackedScene carScene;
        AnimatedSprite car;
        
        // pick a random color
        string colorSuffix = "_" + Colors[GD.Randi() % Colors.Length];
        
        switch (Direction)
        {
            case TrafficDirection.Left:
                carScene = GD.Load<PackedScene>("entities/CarHorizontal.tscn");
                car = carScene.Instance<AnimatedSprite>();
                car.FlipH = true;
                car.Play("running" + colorSuffix);
                break;
            case TrafficDirection.Right:
                carScene = GD.Load<PackedScene>("entities/CarHorizontal.tscn");
                car = carScene.Instance<AnimatedSprite>();
                car.FlipH = false;
                car.Play("running" + colorSuffix);
                break;
            case TrafficDirection.Up:
                carScene = GD.Load<PackedScene>("entities/CarVertical.tscn");
                car = carScene.Instance<AnimatedSprite>();
                car.Play("running_up" + colorSuffix);
                break;
            case TrafficDirection.Down:
                carScene = GD.Load<PackedScene>("entities/CarVertical.tscn");
                car = carScene.Instance<AnimatedSprite>();
                car.Play("running_down" + colorSuffix);
                break;
            default:
                return;
        }
        
        GD.Print("Adding car: ", car);
        car.Position = position;
        AddChild(car);
        _cars.Add(car);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Engine.EditorHint)
        {
            return;
        }

        Vector2 despawnAt = GetDespawnPoint();
        Vector2 respawnAt = GetRespawnPoint();
        
        foreach (AnimatedSprite car in _cars)
        {

            car.Position += GetTrafficVector() * 10;
            
            // car.Position = car.Position.LinearInterpolate()
            // Vector2 bounds;
            // switch (Direction)
            // {
            //     case TrafficDirection.Left:
            //         car.Position += Vector2.Left * 10;
            //         if (car.Position.x <= despawnAt.x)
            //             car.QueueFree();
            //             // car.Position = Vector2.Zero;
            //         break;
            //     case TrafficDirection.Right:
            //         car.Position += Vector2.Right * 10;
            //         if (car.Position.x >= despawnAt.x)
            //             car.QueueFree();
            //             // car.Position = Vector2.Zero;
            //         break;
            //     case TrafficDirection.Up:
            //         car.Position += Vector2.Up * 10;
            //         if (car.Position.y <= despawnAt.y)
            //             car.Position = Vector2.Zero;
            //         break;
            //     case TrafficDirection.Down:
            //         car.Position += Vector2.Down * 10;
            //         if (car.Position.y >= despawnAt.y)
            //             car.Position = Vector2.Zero;
            //         break;
            //     default:
            //         return;
            // }
        }
    }
}
