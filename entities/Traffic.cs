using Godot;
using System;
using System.Linq;

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
    
    private int _numberCars = 10;
    [Export] public int NumberCars {
        get => _numberCars;
        set => SetNumberCars(value);
    }

    public void SetNumberCars(int num)
    {
        _numberCars = num;
        SpawnCars(_direction, _numberCars);
    }
    
    public void SetDirection(TrafficDirection dir)
    {
        _direction = dir;
        SpawnCars(_direction, _numberCars);
    }

    private Vector2 _respawnBoundary;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Engine.EditorHint)
        {
            GD.Print("Hi");
        }
        SpawnCars(_direction, _numberCars);
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
                _respawnBoundary = new Vector2(Position + Vector2.Left * _numberCars * 384);
                break;
            case TrafficDirection.Right:
                carScene = GD.Load<PackedScene>("entities/CarHorizontal.tscn");
                carPrototype = carScene.Instance<Node2D>();
                carPrototypeSprite = carPrototype.GetNode<AnimatedSprite>("AnimatedSprite");
                carPrototypeSprite.FlipH = false;
                carPrototypeSprite.Play("running");
                offset = new Vector2(384, 0);
                _respawnBoundary = new Vector2(Position + Vector2.Right * _numberCars * 384);
                break;
            case TrafficDirection.Up:
                carScene = GD.Load<PackedScene>("entities/CarVertical.tscn");
                carPrototype = carScene.Instance<Node2D>();
                carPrototypeSprite = carPrototype.GetNode<AnimatedSprite>("AnimatedSprite");
                carPrototypeSprite.Play("running_up");
                offset = new Vector2(0, -384);
                _respawnBoundary = new Vector2(Position + Vector2.Up * _numberCars * 384);
                break;
            case TrafficDirection.Down:
                carScene = GD.Load<PackedScene>("entities/CarVertical.tscn");
                carPrototype = carScene.Instance<Node2D>();
                carPrototypeSprite = carPrototype.GetNode<AnimatedSprite>("AnimatedSprite");
                carPrototypeSprite.Play("running_down");
                offset = new Vector2(0, 384);
                _respawnBoundary = new Vector2(Position + Vector2.Down * _numberCars * 384);
                break;
            default:
                return;
        }

        // Remove Existing cars
        foreach (Node2D car in GetChildren())
        {
            car.QueueFree();
        }
        
        // Add new cars        
        foreach (int value in Enumerable.Range(1, NumberCars))
        {
            var car = (Node2D) carPrototype.Duplicate();
            car.Position = currentPosition;
            AddChild(car);
            currentPosition += offset;
        }

        

    }

    public override void _PhysicsProcess(float delta)
    {
        foreach (Node2D car in GetChildren())
        {
            // car.Position = car.Position.LinearInterpolate()
            Vector2 bounds;
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
