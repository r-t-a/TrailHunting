using Godot;
using System;
using TrailHunting.Scripts.Helpers;
using TrailHunting.Scripts.TopDownScripts.Entities;

public class Buffalo : AnimalEntity
{
    [Signal]
    public delegate void LargeGameDead();

    public override void Init()
    {
        HP = 3;
        RunSpeed = 50;
        Connect(nameof(LargeGameDead), Tree.GetNode("TopDownStart"), "_on_Buffalo_LargeGameDead");
    }

    public void WasShot()
    {
        if (HP == 0) 
            return;
        HP -= 1;
        if (HP == 0)
            EmitSignal(nameof(LargeGameDead));
    }

    private void _on_BounceOff_body_entered(Node node)
    {
        if (node.Name != "TileMap")
        {
            Motion = Vector2.Zero - Motion;
            MoveDirection = Motion.VectorToMoveDirection();
        }
    }

    private void _on_Timer_timeout()
    {
        (Motion, MoveDirection) = RandomizeMovement(new Random());
    }

    private void _on_VisibilityNotifier2D_screen_exited()
    {
        QueueFree();
    }
}
