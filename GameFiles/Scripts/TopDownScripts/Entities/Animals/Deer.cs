using Godot;
using System;
using TrailHunting.Scripts.Helpers;
using TrailHunting.Scripts.TopDownScripts.Entities;

public class Deer : AnimalEntity
{
    [Signal]
    public delegate void MediumGameDead();

    public override void Init()
    {
        HP = 1;
        RunSpeed = 90;
        Connect(nameof(MediumGameDead), Tree.GetNode("TopDownStart"), "_on_Deer_MediumGameDead");
    }

    public override void WasShot()
    {
        if (HP == 0) 
            return;
        HP -= 1;
        if (HP == 0)
            EmitSignal(nameof(MediumGameDead));
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