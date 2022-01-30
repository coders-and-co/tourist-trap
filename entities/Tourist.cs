using Godot;
using Duality.states;
using Duality.states.tourist;
using Godot.Collections;
using Collections = System.Collections.Generic;
using Priority_Queue;

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
	public SimplePriorityQueue<Node2D, float> Targets;

	// Tunables
	[Export]
	public bool Debug
	{
		get => _debug;
		set => SetDebug(value);
	}
	[Export] public int Speed = 75;
	[Export] public int SpeedFollow = 175;
	[Export] public int SpeedFollowExcited = 250;
	[Export] public float VisionRadius = 512;
	[Export] public float FollowPollingInterval = 1;
	[Export] public float ComfortDistance = 192;
	[Export] public float MinFollowScore = 20;
	[Export] public float MaxStopFollowScore = 50;
	
	
	public override void _Ready()
	{
		// lookup node references
		Sprites = GetNode<Node2D>("Sprites");
		BodySprite = GetNode<AnimatedSprite>("Sprites/Body");
		FaceSprite = GetNode<AnimatedSprite>("Sprites/Face");
		CameraSprite = GetNode<AnimatedSprite>("Sprites/Camera");
		PointSprite = GetNode<Sprite>("Sprites/Point");
		Audio = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		
		// Create vision area
		Vision = new Area2D();
		Vision.AddChild(new CollisionShape2D());
		Vision.GetChild<CollisionShape2D>(0).Shape = new CircleShape2D();
		((CircleShape2D) Vision.GetChild<CollisionShape2D>(0).Shape).Radius = VisionRadius;
		Vision.CollisionLayer = 0;
		Vision.CollisionMask = 2;
		AddChild(Vision);

		// randomize tourist outfit
		PickRandomFrame(GetNode<AnimatedSprite>("Sprites/Outfit"));
		PickRandomFrame(GetNode<AnimatedSprite>("Sprites/Body Accessory"));
		PickRandomFrame(GetNode<AnimatedSprite>("Sprites/Head Accessory"));
		
		// create state machine
		StateMachine = new FiniteStateMachine<Tourist>(this, new TouristIdleState());
		Targets = new SimplePriorityQueue<Node2D, float>();

		// connect signals
		Vision.Connect("body_entered", this, "TargetSpotted");
		Vision.Connect("area_entered", this, "TargetSpotted");
		Vision.Connect("body_exited", this, "TargetLost");
		Vision.Connect("area_exited", this, "TargetLost");

		Connect("body_entered", this, "OnCollided");
		Map.TouristCount++;
	}

	public void PickRandomFrame(AnimatedSprite sprite, string anim="default") {
		sprite.Frame = (int) GD.Randi() % sprite.Frames.GetFrameCount(anim);
	}
	
	private void SetDebug(bool debug)
	{
		_debug = debug;
		GetNode<TouristDebug>("Debug").Visible = debug;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey key && key.Pressed)
			switch ((KeyList) key.Scancode)
			{
				case KeyList.Tab:
					Debug = !Debug; // turn tourist debug on/off
					break;
			}
	}

	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		switch (StateMachine.CurrentState)
		{
			case TouristMeanderState st:
				LinearDamp = 20;
				AppliedForce = st.Force * 20;
				break;
			case TouristFollowState st:
				LinearDamp = 20;
				AppliedForce = st.Force * 20;
				break;
			case TouristLoadBusState st:
				LinearDamp = 20;
				AppliedForce = st.Force * 15;
				break;
			default:
				AppliedForce = Vector2.Zero;
				LinearDamp = 2000;
				break;
		}
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

	public void OnCollided(Node2D body)
	{
		switch (body)
		{
			case var car when car.IsInGroup("Car"):
				GD.Print("CAR!");
				// ApplyCentralImpulse((Position - car.Position) * 50);
				break;
		}
	}
	
	private float GetInfluence(Node2D t)
	{
		switch (t)
		{
			case IEntity entity:
				return entity.Influence; // 50
			case Flag flag:
				return 40;
			case var bus when bus.IsInGroup("Bus") && !Map.BusTakeMeHome:
				return 60;
			case var bus when bus.IsInGroup("Bus") && Map.BusTakeMeHome:
				return 80;
			case var statue when statue.IsInGroup("Statue"):
				return 110;
			case var feature when feature.IsInGroup("Feature"):
				return 100;
			default:
				return 0;
		}
	}
	public float GetScore(Node2D t)
	{
		var inf = GetInfluence(t);
		var d = t.Position.DistanceTo(Position);
		return Mathf.Max(0, inf - (d * d) / (5000));
	}
	
	public void TargetSpotted(Node2D target)
	{
		// Don't re-add any objects that we've photographed
		if (FeaturesPhotographed.Contains(target.GetInstanceId()))
			return;
		
		if (target.Name == "Influence")
			target = target.GetParent<Node2D>();
		
		switch (target)
		{
			case Player _:
			case Flag _: 
			case NPC npc when npc.Influence > 0:
			case var statue when statue.IsInGroup("Statue"):
			case var feature when feature.IsInGroup("Feature"): 
			case var bus when bus.IsInGroup("Bus"):
				var score = GetScore(target) * -1;
				Targets.EnqueueWithoutDuplicates(target, score);
				break;
			default:
				GD.Print("> Ignoring ", target.Name);
				break;
		}
	}

	public void TargetLost(Node2D target)
	{
		if (target.Name == "Influence")
			target = target.GetParent<Node2D>();
		Targets.TryRemove(target);
	}

	public void AddPhoto(Node2D target)
	{
		FeaturesPhotographed.Add(target.GetInstanceId());
		Targets.TryRemove(target);
	}
	
	public (Node2D?, float) FindTarget()
	{
		if (Targets.Count == 0)
			return (null, 0);
		// loop through each target and update the priority
		foreach(var t in Targets)
			Targets.UpdatePriority(t, GetScore(t) * -1);
		// try to find the best score
		var score = Targets.GetPriority(Targets.First) * -1;
		if (score != 0)
			return (Targets.First, score);
		else
			return (null, 0);
	}

}
