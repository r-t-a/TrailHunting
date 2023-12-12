using Godot;
using System;
using TrailHunting.Scripts;

public class Results : ColorRect
{
    public override void _Ready()
    {
        
    }

    private void _on_Exit_pressed()
    {
        GetTree().ChangeScene(Constants.MainMenu);
    }

    private void _on_Retry_pressed()
    {

    }
}
