using Godot;
using TrailHunting.Scripts.FirstPersonScripts.Entities;
using TrailHunting.Scripts.Managers;

public class Squirrel2 : AnimalEntity
{
    [Signal]
    public delegate void SmallGameDead();

    public override void Init()
    {
        HP = 1;
        Speed = 95;
        AnimatedSprite.Play("Walking");
        Connect(nameof(SmallGameDead), Tree.GetNode("FirstPersonStart"), "_on_Squirrel_SmallGameDead");
        if (Motion == Vector2.Left)
        {
            AnimatedSprite.FlipH = true;
        }
    }

    private void _on_Squirrel_input_event(Node viewport, InputEvent inputEvent, int shape_idx)
    {
        if (Input.IsActionPressed("shoot") && GameManager.PlayerManager.CanShoot())
        {
            UpdateHP();
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
        Speed = 105;
        HP -= 1;
        Timer.Stop();
        if (HP == 0)
        {
            Speed = 0;
            Motion = Vector2.Zero;
            AnimatedSprite.Play("Dead");
            EmitSignal(nameof(SmallGameDead));
        }
    }
}
