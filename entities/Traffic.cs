using Godot;
using System;
using System.Linq;
using Godot.Collections;

[Tool]
public class Traffic : Node2D
{
    public enum TrafficDirection {
        Left,
        Right,
        Up,
        Down
    }
    
    private TrafficDirection _direction = TrafficDirection.Right;
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
        SpawnCars(_direction, _numberCars);
    }
    
    private int _numberCars = 5;
    [Export (PropertyHint.Range, "1,20,")] public int NumberCars {
        get => _numberCars;
        set => SetNumberCars(value);
    }
    public void SetNumberCars(int num)
    {
        _numberCars = num;
        ClearCars();
        SpawnCars(_direction, _numberCars);
    }
    
    private Vector2 _respawnBoundary;
    private Array<Node2D> _cars = new Array<Node2D>();
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Engine.EditorHint)
        {
            
        }
        
        // ClearCars();
        SpawnCars(_direction, _numberCars);
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

    public void SpawnCars(TrafficDirection dir, int num)
    {
        PackedScene carScene;
        Node2D carPrototype;
        AnimatedSprite carPrototypeSprite;
        Vector2 offset; // initial
        Vector2 currentPosition = new Vector2(0, 0);
        
        switch (Direction)
        {
            case TrafficDirection.Left:
                carScene = GD.Load<PackedScene>("entities/CarHorizontal.tscn");
                carPrototype = carScene.Instance<Node2D>();
                carPrototypeSprite = carPrototype.GetNode<AnimatedSprite>("AnimatedSprite");
                carPrototypeSprite.FlipH = true;
                carPrototypeSprite.Play("running");
                offset = new Vector2(-384, 0);
                _respawnBoundary = new Vector2(Vector2.Left * _numberCars * 384);
                break;
            case TrafficDirection.Right:
                carScene = GD.Load<PackedScene>("entities/CarHorizontal.tscn");
                carPrototype = carScene.Instance<Node2D>();
                carPrototypeSprite = carPrototype.GetNode<AnimatedSprite>("AnimatedSprite");
                carPrototypeSprite.FlipH = false;
                carPrototypeSprite.Play("running");
                offset = new Vector2(384, 0);
                _respawnBoundary = new Vector2(Vector2.Right * _numberCars * 384);
                break;
            case TrafficDirection.Up:
                carScene = GD.Load<PackedScene>("entities/CarVertical.tscn");
                carPrototype = carScene.Instance<Node2D>();
                carPrototypeSprite = carPrototype.GetNode<AnimatedSprite>("AnimatedSprite");
                carPrototypeSprite.Play("running_up");
                offset = new Vector2(0, -384);
                _respawnBoundary = new Vector2(Vector2.Up * _numberCars * 384);
                break;
            case TrafficDirection.Down:
                carScene = GD.Load<PackedScene>("entities/CarVertical.tscn");
                carPrototype = carScene.Instance<Node2D>();
                carPrototypeSprite = carPrototype.GetNode<AnimatedSprite>("AnimatedSprite");
                carPrototypeSprite.Play("running_down");
                offset = new Vector2(0, 384);
                _respawnBoundary = new Vector2(Vector2.Down * _numberCars * 384);
                break;
            default:
                return;
        }
        
        // Add new cars        
        foreach (int value in Enumerable.Range(1, NumberCars))
        {
            var car = (Node2D) carPrototype.Duplicate();
            GD.Print("Adding car: ", car);
            car.Position = currentPosition;
            AddChild(car); // Add car to scene
            _cars.Add(car); // Add car to internal array
            currentPosition += offset;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Engine.EditorHint)
        {
            return;
        }
        
        foreach (Node2D car in GetChildren())
        {
            // car.Position = car.Position.LinearInterpolate()
            // Vector2 bounds;
            switch (Direction)
            {
                case TrafficDirection.Left:
                    car.Position += Vector2.Left * 10;
                    if (car.Position.x <= _respawnBoundary.x)
                        car.Position = Vector2.Zero;
                    break;
                case TrafficDirection.Right:
                    car.Position += Vector2.Right * 10;
                    if (car.Position.x >= _respawnBoundary.x)
                        car.Position = Vector2.Zero;
                    break;
                case TrafficDirection.Up:
                    car.Position += Vector2.Up * 10;
                    if (car.Position.y <= _respawnBoundary.y)
                        car.Position = Vector2.Zero;
                    break;
                case TrafficDirection.Down:
                    car.Position += Vector2.Down * 10;
                    if (car.Position.y >= _respawnBoundary.y)
                        car.Position = Vector2.Zero;
                    break;
                default:
                    return;
            }
        }
    }
}
