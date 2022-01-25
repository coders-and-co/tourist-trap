using Godot;
using System;
using Duality.states;
using Duality.states.player;

public class Player : KinematicBody2D, IAttractive
{
	private PackedScene _flagScene = GD.Load<PackedScene>("entities/Flag.tscn");
	public Node2D Sprites;
	public AnimatedSprite BodySprite;
	public Sprite FlagSprite;
	
	// Finite State Machine
	public FiniteStateMachine<Player> StateMachine;
	
	[Export] public int Speed = 240;
	public bool HasFlag = true;

	public float GetBaseAttraction() { return 10; }
	
	public override void _Ready()
	{
		// lookup references
		Sprites = GetNode<Node2D>("Sprites");
		BodySprite = GetNode<AnimatedSprite>("Sprites/Body");
		FlagSprite = GetNode<Sprite>("Sprites/Flag");
		
		// Create FSM
		StateMachine = new FiniteStateMachine<Player>(this, new PlayerIdleState());
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

	public void ThrowFlag(Vector2 to)
	{
		GD.Print("Throwing flag to ", to);
		HasFlag = false;
		
		// Add Flag to Game 
		Flag flag = _flagScene.Instance<Flag>();
		var entities = GetTree().Root.GetNode<Node2D>("Game/Entities");
		entities.AddChild(flag);
		// Call Throw method for flag
		flag.Throw(Position, to);
	}

	public void CollectFlag(Flag flagNode)
	{
		HasFlag = true;
		FlagSprite.Visible = true;
		flagNode.QueueFree();
	}
	
}
