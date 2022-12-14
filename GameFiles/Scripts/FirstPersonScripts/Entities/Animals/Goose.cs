using Godot;
using TrailHunting.Scripts.FirstPersonScripts.Entities;
using TrailHunting.Scripts.Managers;

public class Goose : AnimalEntity
{
    [Signal]
    public delegate void SmallGameDead();

    public override void Init()
    {
        HP = 1;
        Speed = 80;
        Connect(nameof(SmallGameDead), Tree.GetNode("FirstPersonStart"), "_on_Goose_SmallGameDead");
    }

    private void _on_Goose_input_event(Node viewport, InputEvent inputEvent, int shape_idx)
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
            Speed = 150;
            AnimatedSprite.Play("Dead");
        }
    }

    private void _on_VisibilityNotifier2D_screen_exited()
    {
        QueueFree();
    }

    public void UpdateHP()
    {
        if (HP == 0)
        {
            return;
        }
        HP -= 1;
        if (HP == 0)
        {
            Motion = Motion == Vector2.Right ? new Vector2(1, 1) : new Vector2(-1, 1);
            AnimatedSprite.Play("Dying");
            EmitSignal(nameof(SmallGameDead));
        }
    }
}
