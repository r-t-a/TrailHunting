using Godot;
using TrailHunting.Scripts.FirstPersonScripts.Entities;
using TrailHunting.Scripts.Managers;

public class Bear2 : AnimalEntity
{
    [Signal]
    public delegate void MediumLargeGameDead();

    public override void Init()
    {
        HP = 2;
        Speed = 40;
        Connect(nameof(MediumLargeGameDead), Tree.GetNode("FirstPersonStart"), "_on_Bear_MediumLargeGameDead");
    }

    private void _on_Bear_input_event(Node viewport, InputEvent inputEvent, int shape_idx)
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
        Speed = 60;
        HP -= 1;
        Timer.Stop();
        if (HP == 0)
        {
            Speed = 0;
            Motion = Vector2.Zero;
            AnimatedSprite.Play("Dying");
            EmitSignal(nameof(MediumLargeGameDead));
        }
    }
}
