using Godot;
using System;
using TrailHunting.Scripts.FirstPersonScripts.Entities;
using TrailHunting.Scripts.Managers;

public class Caribou : AnimalEntity
{
    [Signal]
    public delegate void MediumGameDead();

    public override void Init()
    {
        HP = 1;
        Speed = 80;
        Connect(nameof(MediumGameDead), Tree.GetNode("FirstPersonStart"), "_on_Caribou_MediumGameDead");
    }

    private void _on_Caribou_input_event(Node viewport, InputEvent inputEvent, int shape_idx)
    {
        if (Input.IsActionPressed("shoot") && GameManager.PlayerManager.CanShoot())
        {
            UpdateHP();
        }
    }

    private void _on_AnimatedSprite_animation_finished()
    {
        if (AnimatedSprite.Animation == "Dying")
        {
            Speed = 0;
            AnimatedSprite.Play("Dead");
        }
    }

    private void _on_VisibilityNotifier2D_screen_exited()
    {
        QueueFree();
    }

    private void _on_MoveTimer_timeout()
    {
        SetMotion(RandomizeMovement());
    }

    public void UpdateHP()
    {
        if (HP == 0)
        {
            return;
        }
        SetMotion(Vector2.Left);
        Speed = 90;
        HP -= 1;
        Timer.Stop();
        if (HP == 0)
        {
            Speed = 0;
            Motion = Vector2.Zero;
            AnimatedSprite.Play("Dying");
            EmitSignal(nameof(MediumGameDead));
        }
    }
}
