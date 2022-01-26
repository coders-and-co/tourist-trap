using Godot;
using System;
using Duality.states;
using Duality.states.player;

public class Player : KinematicBody2D, IEntity
{
    // Resources and nodes
    private PackedScene _flagScene = GD.Load<PackedScene>("entities/Flag.tscn");
    public Area2D InfluenceArea;
    public Area2D InteractArea;
    public Node2D Sprites;
    public AnimatedSprite BodySprite;
    public Sprite FlagSprite;
    public AudioStreamPlayer2D Audio;
    
    // State variables
    public FiniteStateMachine<Player> StateMachine;
    public bool HasFlag = true;
    
    // Tunables
    [Export] public int Speed = 240;
    [Export] public int Influence = 8;
    int IEntity.Influence { get => Influence; }
    
    public override void _Ready()
    {
        // lookup node references
        InfluenceArea = GetNode<Area2D>("Influence");
        InteractArea = GetNode<Area2D>("Interact");
        Sprites = GetNode<Node2D>("Sprites");
        BodySprite = GetNode<AnimatedSprite>("Sprites/Body");
        FlagSprite = GetNode<Sprite>("Sprites/Flag");
        Audio = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        
        // create state machine
        StateMachine = new FiniteStateMachine<Player>(this, new PlayerIdleState());

        // signals
        InteractArea.Connect("body_entered", this, "OnTouchSomething");
        InteractArea.Connect("area_entered", this, "OnTouchSomething");
    }
    
    public override void _PhysicsProcess(float delta)
    {
        StateMachine.Update(delta);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if ((ButtonList) mouseEvent.ButtonIndex == ButtonList.Left) {
                GD.Print("Click at: ", GetGlobalMousePosition());
                StateMachine.OnLeftClick(GetGlobalMousePosition());
            }
        }
    }
    
    public Vector2 GetMovementVector()
    {
        Vector2 movement = Vector2.Zero;
        if (Input.IsActionPressed("move_left"))
            movement.x = -1;
        else if (Input.IsActionPressed("move_right"))
            movement.x = 1;
        if (Input.IsActionPressed("move_up"))
            movement.y = -1;
        else if (Input.IsActionPressed("move_down"))
            movement.y = 1;
        return movement;
    }

	public void OnTouchSomething(Node body)
	{
		if (body.IsInGroup("Flag"))
		{
			var flag = (Flag) body;
			if (!flag.Moving)
				CollectFlag(flag);
		}
	}

    public void Shout()
    {
        var inf = InfluenceArea.GetChild<CollisionShape2D>(0); // .Shape;
        // GD.Print("Current radius: ", inf.Radius);
        // inf.Radius = 256;
        Tween tween = new Tween();
        GetTree().Root.AddChild(tween);
        // AddChild(tween);
        tween.InterpolateProperty(inf, "shape:radius", 32, 256, 1.0f);
        tween.InterpolateProperty(this, "Influence", 8, 32, 1.0f);
        tween.InterpolateProperty(inf, "shape:radius", 256, 32, 5.0f, Tween.TransitionType.Linear, Tween.EaseType.InOut, 2.5f);
        tween.InterpolateProperty(this, "Influence", 32, 8, 5.0f, Tween.TransitionType.Linear, Tween.EaseType.InOut, 2.5f);
        tween.Start();
        // tween.Connect()
        // inf.Radius = 
        // EmitSignal(nameof(PlayerShout), this);
    }

    public void ThrowFlag(Vector2 to)
    {
        GD.Print("Throwing flag to ", to);
        HasFlag = false;
        
        // Add Flag to Game 
        Flag flag = _flagScene.Instance<Flag>();
        var entities = GetTree().Root.GetNode<Node2D>("Game/Entities");
        entities.AddChild(flag);
        // Call Throw method for flag
        flag.FlyBetween(Position, to);
    }

    public void CollectFlag(Flag flagNode)
    {
        HasFlag = true;
        FlagSprite.Visible = true;
        flagNode.QueueFree();
    }
    
}
