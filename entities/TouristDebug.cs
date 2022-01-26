using Godot;
using System;
using Duality.states.tourist;

public class TouristDebug : Node2D
{

    public Tourist Tourist;
    public Font DebugFont;
    public Label DebugLabel;
    
    static readonly Color Secondary = Color.ColorN("midnightblue", 0.75f);
    static readonly Color Primary = Color.ColorN("springgreen", 0.75f);
    static readonly Color DarkAlpha = Color.ColorN("dimgray", 0.1f);
    static readonly Color LightAlpha = Color.ColorN("white", 0.1f);
    
    public override void _Ready()
    {
        Tourist = GetParent<Tourist>();
        // GetNode<Node2D>("Debug").Visible = true;
        DebugLabel = GetNode<Label>("PanelContainer/Label");
        DebugFont = new Label().GetFont("");
    }
    
    public override void _Process(float delta)
    {
        Update();
    }

    public override void _Draw()
    {
        var state = Tourist.StateMachine.CurrentState;
        var stateType = state.GetType();
        
        DebugLabel.Text = string.Format($"{state.GetName()} {state.GetDebugState()}");

        // Draw vision circle
        var circle = (CircleShape2D) Tourist.Vision.GetChild<CollisionShape2D>(0).Shape;
        // DrawCircle(Vector2.Zero, circle.Radius, DarkAlpha);
        // DrawArc(Vector2.Zero, circle.Radius, 0, Mathf.Pi * 2, 40, LightAlpha, 0.5f, true);
		
        foreach (var t in Tourist.Targets)
        {
            var score = Tourist.Targets.GetPriority(t) * -1;
            var dv = t.Position - Tourist.Position;
            var dist = dv.Length();
            var endPos = dv.Clamped(dist - 4);
            var startPos = dv.Clamped(4); 
            var textPos = endPos / 2 + new Vector2(-8, 5);
            var lineColor = Secondary;
            var textColor = Colors.White;
            
            // var badgeColor = Black;
			
            if (stateType == typeof(TouristFollowState))
            {
                var followState = (TouristFollowState) state;
                if (t == followState.GetDebugTarget())
                {
                    lineColor = Primary;
                    textColor = Colors.Black;
                }
            }
			
            // Draw actual line
            DrawLine(startPos, endPos, lineColor, 1.5f, true);
            // Draw circle ends
            DrawCircle(Vector2.Zero, 4, lineColor);
            DrawCircle(dv, 4, lineColor);
            // Draw text badge
            DrawRect(new Rect2(textPos.x - 8, textPos.y + 3, 32, -16), lineColor);
            DrawString(DebugFont, textPos, string.Format($"{score:N0}"), textColor);
        }
    }
}
