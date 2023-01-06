using Godot;
using TrailHunting.Scripts;

public class Intro : Control
{
    [Export]
    protected NodePath AnimatedNodePath;
    [Export]
    protected NodePath TimerNodePath;

    private AnimatedSprite animatedSprite;
    private Timer startTimer;
    private int escapeCount = 0;
    private int waitTime = 3;
    private float time = 0;

    public override void _Ready()
    {
        animatedSprite = GetNodeOrNull<AnimatedSprite>(AnimatedNodePath);
        startTimer = GetNodeOrNull<Timer>(TimerNodePath);
        animatedSprite.Play(Constants.Idle);
        animatedSprite.Playing = true;
    }

    public override void _Process(float delta)
    {
        time += delta;
        if (time > waitTime)
        {
            if (animatedSprite.Animation == Constants.Idle)
            {
                animatedSprite.Play(Constants.Start);
            }
            time = 0;
        }
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed(Constants.Cancel))
        {
            escapeCount++;
            if (escapeCount == 3)
            {
                GetTree().ChangeScene(Constants.MainMenu);
            }
        }
    }

    private void _on_AnimatedSprite_animation_finished()
    {
        if (animatedSprite.Animation == Constants.Idle)
        {
            return;
        }
        if (animatedSprite.Animation == Constants.Start)
        {
            animatedSprite.Frame = 14;
            animatedSprite.Playing = false;
            startTimer.Start();
        }
    }

    private void _on_StartTimer_timeout()
    {
        ChangeToMainMenu();
    }

    private void ChangeToMainMenu()
    {
        GetTree().ChangeScene(Constants.MainMenu);
    }
}
