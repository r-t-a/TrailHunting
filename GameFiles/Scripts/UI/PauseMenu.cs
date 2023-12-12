using Godot;
using TrailHunting.Scripts;

public class PauseMenu : Control
{
    #region Overrides
    public override void _Ready() => PauseMode = PauseModeEnum.Process;

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed(Constants.Pause))
        {
            GetTree().Paused = !GetTree().Paused;
            if (GetTree().Paused)
                Show();
            else
                Hide();
        }
    }
    #endregion

    #region Events
    private void _on_Resume_pressed()
    {
        Hide();
        GetTree().Paused = false;
    }

    private void _on_Quit_pressed()
    {
        GetTree().ChangeScene(Constants.MainMenu);
        GetTree().Paused = false;
    }
    #endregion
}