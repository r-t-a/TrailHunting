using Godot;
using TrailHunting.Scripts;

public class MainMenu : Control
{
    public AnimatedSprite AnimatedSprite;

    public override void _Ready()
    {
        AnimatedSprite = GetNodeOrNull<AnimatedSprite>("AnimatedSprite");
        AnimatedSprite.Play("Idle");
        AnimatedSprite.Playing = true;
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("num1"))
        {
            // Show next options when First Person is done
            //AnimatedSprite.Play("NextOptions");
            GetTree().ChangeScene(Constants.TopDownStart);
        }
        if (inputEvent.IsActionPressed("num2"))
        {
            //GetTree().ChangeScene(Constants.TopDownStart);
        }
        if (inputEvent.IsActionPressed("num3"))
        {
            //GetTree().ChangeScene(Constants.FirstPersonStart);
        }
        if (inputEvent.IsActionPressed("num8"))
        {

        }
        if (inputEvent.IsActionPressed("num9"))
        {
            GetTree().Quit();
        }
    }
}
