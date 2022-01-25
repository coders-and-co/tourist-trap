using Godot;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Duality.states;
using Duality.states.player;
using Duality.states.tourist;
using Godot.Collections;

public class Tourist : RigidBody2D, IAttractive
{
	// Node References
	public Node2D Sprites;
	public AnimatedSprite BodySprite;
	public AnimatedSprite FaceSprite;
	public AnimatedSprite CameraSprite;
	public Area2D Vision;
	public Label Label;
	
	// Finite State Machine
	public FiniteStateMachine<Tourist> StateMachine;
	public Array<int> FeaturesPhotographed = new Array<int>();
	
	[Export]
	public int Speed = 80;

	// Target attraction values 
	private readonly Dictionary<string, int> _target = new Dictionary<string, int>()
	{
		// {"Tourist", 4},
		{"Flag", 8},
		{"Player", 12},
		{"Bus", 24},
		{"Feature", 60}
	};

	public float GetBaseAttraction() { return 4; }
	
	public override void _Ready()
	{
		GD.Randomize();
		
		// lookup node references
		Label = GetNode<Label>("Label");
		Vision = GetNode<Area2D>("Vision");
		Sprites = GetNode<Node2D>("Sprites");
		BodySprite = GetNode<AnimatedSprite>("Sprites/Body");
		FaceSprite = GetNode<AnimatedSprite>("Sprites/Face");
		CameraSprite = GetNode<AnimatedSprite>("Sprites/Camera");
		var outfitSprite = GetNode<AnimatedSprite>("Sprites/Outfit");
		var bodyAccessorySprite = GetNode<AnimatedSprite>("Sprites/Body Accessory");
		var headAccessorySprite = GetNode<AnimatedSprite>("Sprites/Head Accessory");
		
		// randomize tourist outfit
		// PickRandomFrame(FaceSprite, "happy");
		PickRandomFrame(outfitSprite);
		PickRandomFrame(bodyAccessorySprite);
		PickRandomFrame(headAccessorySprite);

		// var rootNode = GetTree().Root.GetNode<Game>("Game"); 
		StateMachine = new FiniteStateMachine<Tourist>(this, new TouristIdleState());
	}

	public void PickRandomFrame(AnimatedSprite sprite, string anim="default") {
		sprite.Frame = (int) GD.Randi() % sprite.Frames.GetFrameCount(anim);
	}
	
	public override void _PhysicsProcess(float delta)
	{
		
		StateMachine.Update(delta);
		Label.Text = StateMachine.CurrentState.GetName();
		
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
	
	public Node2D FindTarget()
	{
		Array<Node2D> targetList = new Array<Node2D>();
		Node2D currentTarget = null;
		float currentScore = 0;
		
		// Get overlapping bodies
		foreach (PhysicsBody2D body in Vision.GetOverlappingBodies()) {
			// typeof(IAttractive).IsAssignableFrom()
			if (!FeaturesPhotographed.Contains((int) body.GetInstanceId()))
				targetList.Add(body);
		}

		// Area check
		foreach (Area2D area in Vision.GetOverlappingAreas())
		{
			// Only consider a feature for target if it has not been photographed 
			if (area.IsInGroup("Feature") && !FeaturesPhotographed.Contains((int) area.GetInstanceId()))
				targetList.Add(area);
		}
		
		// Compare targets
		foreach (Node2D t in targetList)
		{
			String group = FindGroup(t);
			if (group != "")
			{
				// calculate potential score!
				var dist = t.Position.DistanceTo(Position);
				
				if ((group == "Player" || group == "Flag") && dist <= 64)
					continue;
				
				float potentialScore = _target[group] / dist;
				if (potentialScore > currentScore)
				{
					currentTarget = t;
					currentScore = potentialScore;
				}
			}
		}
		return currentTarget;
	}
}
