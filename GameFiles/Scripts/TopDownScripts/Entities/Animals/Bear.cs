using Godot;
using System;
using TrailHunting.Scripts.Helpers;
using TrailHunting.Scripts.TopDownScripts.Entities;

public class Bear : AnimalEntity
{
    [Signal]
    public delegate void MedLargeGameDead();

    public override void Init()
    {
        HP = 2;
        RunSpeed = 55;
        Connect(nameof(MedLargeGameDead), Tree.GetNode("TopDownStart"), "_on_Bear_MedLargeGameDead");
    }

    public override void WasShot()
    {
        if (HP == 0) 
            return;
        HP -= 1;
        if (HP == 0) 
            EmitSignal(nameof(MedLargeGameDead));
    }

    private void _on_BounceOff_body_entered(Node node)
    {
        if (node.Name != "TileMap")
        {
            Motion = Vector2.Zero - Motion;
            MoveDirection = Motion.VectorToMoveDirection();
        }
    }

    private void _on_Timer_timeout() => (Motion, MoveDirection) = RandomizeMovement(new Random());

    private void _on_VisibilityNotifier2D_screen_exited() => QueueFree();
}