using Godot;
using System;
using Duality.states;
using Duality.states.tourist;
using Godot.Collections;
using Collections = System.Collections.Generic;
using Priority_Queue;
using Object = Godot.Object;

public class Tourist : RigidBody2D
{
	// Resources and nodes
	public Node2D Sprites;
	public AnimatedSprite BodySprite;
	public AnimatedSprite FaceSprite;
	public AnimatedSprite CameraSprite;
	public Sprite PointSprite;
	public Area2D Vision;
	public AudioStreamPlayer2D Audio;
	
	// State variables
	private bool _debug;
	public FiniteStateMachine<Tourist> StateMachine;
	public Array<ulong> FeaturesPhotographed = new Array<ulong>();
	public SimplePriorityQueue<Node2D, float> Targets = new SimplePriorityQueue<Node2D, float>();

	// Tunables
	[Export]
	public bool Debug
	{
		get { return _debug; }
		set { SetDebug(value); }
	}
	[Export] public int Speed = 75;
	[Export] public int SpeedFollow = 175;
	[Export] public int SpeedFollowExcited = 250;
	[Export] public float FollowPollingInterval = 1.0f;

	// Target attraction values 
	private readonly Dictionary<string, int> _target = new Dictionary<string, int>()
	{
		// {"Tourist", 4},
		{"Flag", 5000},
		{"Player", 8000},
		{"Bus", 16000},
		{"Feature", 32000}
	};

	private void SetDebug(bool debug)
	{
		_debug = debug;
		GetNode<TouristDebug>("Debug").Visible = debug;
	}

	public override void _Input(InputEvent @event)
	{
		// bail out unless it's a key down event
		if (!(@event is InputEventKey key) || !key.Pressed) return;
		switch ((KeyList) key.Scancode)
		{
			case KeyList.Tab:
				Debug = !Debug; // turn tourist debug on/off
				break;
		}
	}

	public override void _Ready()
	{
		// lookup node references
		Vision = GetNode<Area2D>("Vision");
		Sprites = GetNode<Node2D>("Sprites");
		BodySprite = GetNode<AnimatedSprite>("Sprites/Body");
		FaceSprite = GetNode<AnimatedSprite>("Sprites/Face");
		CameraSprite = GetNode<AnimatedSprite>("Sprites/Camera");
		PointSprite = GetNode<Sprite>("Sprites/Point");
		Audio = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		
		// randomize tourist outfit
		PickRandomFrame(GetNode<AnimatedSprite>("Sprites/Outfit"));
		PickRandomFrame(GetNode<AnimatedSprite>("Sprites/Body Accessory"));
		PickRandomFrame(GetNode<AnimatedSprite>("Sprites/Head Accessory"));
		
		// create state machine
		StateMachine = new FiniteStateMachine<Tourist>(this, new TouristIdleState());

		// connect signals
		Vision.Connect("body_entered", this, "TargetSpotted");
		Vision.Connect("area_entered", this, "TargetSpotted");
		Vision.Connect("body_exited", this, "TargetLost");
		Vision.Connect("area_exited", this, "TargetLost");
	}

	public void PickRandomFrame(AnimatedSprite sprite, string anim="default") {
		sprite.Frame = (int) GD.Randi() % sprite.Frames.GetFrameCount(anim);
	}
	
	public override void _PhysicsProcess(float delta)
	{
		StateMachine.Update(delta);
		// Flip sprites if moving right
		if (LinearVelocity.x > 5)
			Sprites.Scale = new Vector2(-1, 1);
		else if (LinearVelocity.x < -5)
			Sprites.Scale = new Vector2(1, 1);
	}
	
	public string FindGroup(Node2D body)
	{
		if (body.IsInGroup("Player"))
			return "Player";
		// else if (body.IsInGroup("Tourist"))
		// 	return "Tourist";
		else if (body.IsInGroup("Flag"))
			return "Flag";
		else if (body.IsInGroup("Feature"))
			return "Feature";
		else if (body.IsInGroup("Bus"))
			return "Bus";
		else 
			return "";
	}


	public void TargetSpotted(Node2D target)
	{
		// Don't re-add any objects that we've photographed
		if (FeaturesPhotographed.Contains(target.GetInstanceId()))
			return;
			
		switch (FindGroup(target))
		{
			case "Player":
			case "Flag":
			case "Bus":
			case "Feature":
				Targets.EnqueueWithoutDuplicates(target, GetPotentialScoreFor(target) * -1);
				break;
		}
	}
	
	public void TargetLost(Node2D target) 
	{
		Targets.TryRemove(target);
	}

	public void AddPhoto(Node2D target)
	{
		FeaturesPhotographed.Add(target.GetInstanceId());
		Targets.TryRemove(target);
	}

	public float GetPotentialScoreFor(Node2D t)
	{
		var group = FindGroup(t);
		var dist = t.Position.DistanceTo(Position);

		switch (FindGroup(t))
		{
			case "Player":
			case "Flag":
				// ignore player/flag if close by
				if (dist <= 64)
					return 0;
				break;
		}
		return _target[group] / dist;
	}
	
	public Node2D FindTarget()
	{
		if (Targets.Count == 0)
			return null;
		
		// recalculate the scores in the priority queue before we return the best
		foreach (var target in Targets)
		{
			var score = GetPotentialScoreFor(target);
			Targets.UpdatePriority(target, score * -1);
		}
		
		// return the best score from Targets (or null if the best score is zero)
		if (GetPotentialScoreFor(Targets.First) != 0)
			return Targets.First;
		else
			return null;

	}

}
