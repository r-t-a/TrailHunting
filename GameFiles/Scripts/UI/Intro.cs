using Godot;
using TrailHunting.Scripts;

public class Intro : Control
{
	public AnimatedSprite AnimatedSprite;
	public Timer StartTimer;

	private int escapeCount = 0;
	private int waitTime = 3;
	private float time = 0;

	public override void _Ready()
	{
		AnimatedSprite = GetNodeOrNull<AnimatedSprite>("AnimatedSprite");
		StartTimer = GetNodeOrNull<Timer>("StartTimer");
		AnimatedSprite.Play("Idle");
		AnimatedSprite.Playing = true;
	}

	public override void _Process(float delta)
	{
		time += delta;
		if (time > waitTime)
		{
			if (AnimatedSprite.Animation == "Idle") 
			{
				AnimatedSprite.Play("Start");
			}
			time = 0;
		}
	}

	public override void _Input(InputEvent inputEvent)
	{ 
		if (inputEvent.IsActionPressed("ui_cancel"))
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
		if (AnimatedSprite.Animation == "Idle")
		{
			return;
		}
		if (AnimatedSprite.Animation == "Start")
		{
            AnimatedSprite.Frame = 14;
            AnimatedSprite.Playing = false;
            StartTimer.Start();
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
