using Godot;
using System;
using Duality.states;
using Duality.states.player;

public class Player : KinematicBody2D
{
    // Finite State Machine
    public FiniteStateMachine<Player> FSM;
    
    [Export] public int Speed = 240;
    public bool HasFlag = true;
    
    // Node References
    public Node2D Sprites;
    public AnimatedSprite BodySprite;
    public Sprite FlagSprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Save Node references
        Sprites = GetNode<Node2D>("Sprites");
        BodySprite = GetNode<AnimatedSprite>("Sprites/Body");
        FlagSprite = GetNode<Sprite>("Sprites/Flag");
        
        // Create FSM
        var rootNode = GetTree().Root.GetNode<Game>("Game"); 
        FSM = new FiniteStateMachine<Player>(rootNode, this, new PlayerIdleState());
    }
    
    public override void _PhysicsProcess(float delta)
    {
        FSM.Update(delta);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if ((ButtonList) mouseEvent.ButtonIndex == ButtonList.Left) {
                GD.Print("Click at: ", mouseEvent.Position);
                FSM.OnLeftClick(mouseEvent.Position);
            }
        }
    }
    
    

    public void OnAnimationFinished()
    {
        var anim = BodySprite.Animation;
        if (BodySprite.Frames.GetAnimationLoop(anim) == false)
            BodySprite.Stop();
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


}
