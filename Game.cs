using Godot;
using System;

public class Game : Node2D
{
	
	[Signal] delegate void PlayerShout(Player player);
	[Signal] delegate void PlayerInteract(Player player);
	
	public static Vector2 ScaleVectorNormal = new Vector2(1, 1);
	public static Vector2 ScaleVectorFlipped = new Vector2(-1, 1);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

}
