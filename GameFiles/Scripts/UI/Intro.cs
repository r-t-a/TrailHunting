using Godot;
using System;
using System.Threading.Tasks;
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
                AnimatedSprite.Play("Enter");
            }
            if (AnimatedSprite.Animation == "EnterIdle")
            {
                AnimatedSprite.Play("OpenDisk");
            }
            if (AnimatedSprite.Animation == "IdleA")
            {
                AnimatedSprite.Play("Boot");
            }
            time = 0;
        }
    }

    public override void _Input(InputEvent inputEvent)
    { 
        //TODO switch to timed
        if (inputEvent.IsActionPressed("ui_cancel"))
        {
            escapeCount++;
            if (escapeCount == 3)
            {
                if (AnimatedSprite.Animation != "Boot")
                {
                    AnimatedSprite.Play("Boot");
                }
            }
        }
    }

    private void _on_AnimatedSprite_animation_finished()
    {
        if (AnimatedSprite.Animation == "Idle")
        {
            return;
        }
        if (AnimatedSprite.Animation == "Enter")
        {
            AnimatedSprite.Play("EnterIdle");
        }
        if (AnimatedSprite.Animation == "OpenDisk")
        {
            AnimatedSprite.Play("IdleA");
        }
        if (AnimatedSprite.Animation == "Boot")
        {
            AnimatedSprite.Frame = 3;
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
