using Godot;
using System;
using Duality.states;
using Duality.states.player;
using Duality.states.tourist;
using Godot.Collections;

public class Tourist : RigidBody2D
{
	// Finite State Machine
	public FiniteStateMachine<Tourist> FSM;
	public Node2D PlayerToFollow = null;
	public Array<int> FeaturesPhotographed;
	
	[Export]
	public int Speed = 80;
	
	// Node References
	private Node2D _sprites;
	public AnimatedSprite BodySprite;
	private AnimatedSprite _faceSprite;
	private AnimatedSprite _outfitSprite;
	private AnimatedSprite _bodyAccessorySprite;
	private AnimatedSprite _headAccessorySprite;
	public AnimatedSprite CameraSprite;
	private Area2D _vision;

	private Dictionary<string, int> _target;
	
	private Vector2 _force = new Vector2(50, 0);

	public override void _Ready()
	{
		GD.Randomize();
		
		// save node references
		_sprites = GetNode<Node2D>("Sprites");
		BodySprite = GetNode<AnimatedSprite>("Sprites/Body");
		_faceSprite = GetNode<AnimatedSprite>("Sprites/Face");
		_outfitSprite = GetNode<AnimatedSprite>("Sprites/Outfit");
		_bodyAccessorySprite = GetNode<AnimatedSprite>("Sprites/Body Accessory");
		_headAccessorySprite = GetNode<AnimatedSprite>("Sprites/Head Accessory");
		CameraSprite = GetNode<AnimatedSprite>("Sprites/Camera");

		_vision = GetNode<Area2D>("Vision");
		FeaturesPhotographed = new Array<int>();
		
		_target = new Dictionary<string, int>();
		
		_target.Add("Player", 10);
		_target.Add("Flag", 8);
		_target.Add("Feature", 60);

		// randomize tourist
		PickRandomFrame(_faceSprite, "happy");
		PickRandomFrame(_outfitSprite);
		PickRandomFrame(_bodyAccessorySprite);
		PickRandomFrame(_headAccessorySprite);

		var rootNode = GetTree().Root.GetNode<Game>("Game"); 
		FSM = new FiniteStateMachine<Tourist>(rootNode, this, new TouristIdleState());
	}

	private void PickRandomFrame(AnimatedSprite sprite, string anim="default") {
		sprite.Frame = (int) GD.Randi() % sprite.Frames.GetFrameCount(anim);
	}
	
	public override void _PhysicsProcess(float delta)
	{
		
		FSM.Update(delta);
		
		if (LinearVelocity.x > 0)
		{
			_sprites.Scale = new Vector2(-1, 1);
		}
		else {
			_sprites.Scale = new Vector2(1, 1);
		}
	}

	public Node2D FindTarget()
	{
		float currentScore = 0;
		if (PlayerToFollow != null)
		{
			currentScore = _target[PlayerToFollow.Name] / PlayerToFollow.Position.DistanceTo(Position);
		}

		Array<Node2D> targetListArray = new Array<Node2D>();
		
		foreach (PhysicsBody2D body in _vision.GetOverlappingBodies()) 
			targetListArray.Add(body);

		foreach (Area2D area in _vision.GetOverlappingAreas())
		{
			// Only consider a feature for target if it has not been photographed 
			if (area.IsInGroup("Feature") && !FeaturesPhotographed.Contains(area.GetRid().GetId()))
			{
				targetListArray.Add(area);
			}
		}
		foreach (Node2D body in targetListArray)
		{
			
			if (_target.ContainsKey(body.Name))
			{
				float potentialScore = _target[body.Name] / body.Position.DistanceTo(Position);

				if (potentialScore > currentScore)
				{
					return body;
				}
			}
		}
		return PlayerToFollow;

	}

	public void OnBodySpotted(Node body)
	{
		if (body.IsInGroup("Player"))
			PlayerToFollow =  (Node2D) body;
		else if (body.IsInGroup("Flag"))
			PlayerToFollow =  (Node2D) body;
	}

	public void OnBodyExited(Node body)
	{
		if (body == PlayerToFollow)
		{
			PlayerToFollow = null;
			PlayerToFollow = FindTarget();
		}
	}
	
	public void OnAreaSpotted(Area2D area)
	{
		//FIX
		if (area == PlayerToFollow)
		{
			PlayerToFollow = null;
			PlayerToFollow = FindTarget();
		}
	}
	
}
