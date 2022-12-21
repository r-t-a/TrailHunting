using Godot;
using TrailHunting.Scripts;
using TrailHunting.Scripts.Managers;

public class MainMenu : Control
{
    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("num1"))
        {
            if (GameManager.IsTypeA)
            {
                GetTree().ChangeScene(Constants.FirstPersonStart);
            }
            else
            {
                GetTree().ChangeScene(Constants.TopDownStart);
            }
        }
        if (inputEvent.IsActionPressed("num8"))
        {
            //Show options
        }
        if (inputEvent.IsActionPressed("num9"))
        {
            GetTree().Quit();
        }
    }
}
