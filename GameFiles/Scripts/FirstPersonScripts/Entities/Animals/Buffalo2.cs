using Godot;
using TrailHunting.Scripts.FirstPersonScripts.Entities;
using TrailHunting.Scripts.Managers;

public class Buffalo2 : AnimalEntity
{
    [Signal]
    public delegate void LargeGameDead();

    public override void Init()
    {
        HP = 3;
        Speed = 30;
        Connect(nameof(LargeGameDead), Tree.GetNode("FirstPersonStart"), "_on_Buffalo_LargeGameDead");
    }

    private void _on_Buffalo_input_event(Node viewport, InputEvent inputEvent, int shape_idx)
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
        Speed = 50;
        HP -= 1;
        Timer.Stop();
        if (HP == 0)
        {
            Speed = 0;
            Motion = Vector2.Zero;
            AnimatedSprite.Play("Dying");
            EmitSignal(nameof(LargeGameDead));
        }
    }
}
