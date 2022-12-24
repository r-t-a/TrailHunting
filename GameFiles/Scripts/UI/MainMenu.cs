using Godot;
using TrailHunting.Scripts;
using TrailHunting.Scripts.Managers;

public class MainMenu : Control
{
    public WindowDialog OptionsDialog;
    public OptionButton GameTypeOptionButton;

    public override void _Ready()
    {
        OptionsDialog = GetNodeOrNull<WindowDialog>("CanvasLayer/OptionsDialog");
        GameTypeOptionButton = OptionsDialog.GetNodeOrNull<OptionButton>("MarginContainer/VBoxContainer/GameTypeContainer/GameTypeButton");
        if (GameManager.IsFirstPersonStyle)
        {
            GameTypeOptionButton.Selected = 1;
        }
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("num1"))
        {
            if (GameManager.IsFirstPersonStyle)
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
            if (!OptionsDialog.Visible)
            {
                OptionsDialog.Visible = true;
            }
        }
        if (inputEvent.IsActionPressed("num9"))
        {
            GetTree().Quit();
        }
    }

    private void _on_GameTypeButton_item_selected(int selected)
    {
        if (selected == 0)
        {
            GameManager.SetGameType(false);
        }
        else
        {
            GameManager.SetGameType(true);
        }
    }
}
