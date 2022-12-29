using Godot;
using TrailHunting.Scripts;
using TrailHunting.Scripts.Managers;

public class MainMenu : Control
{
    public WindowDialog OptionsDialog;
    public OptionButton GameTypeButton;
    public OptionButton GameOptionButton;

    public override void _Ready()
    {
        OptionsDialog = GetNodeOrNull<WindowDialog>("CanvasLayer/OptionsDialog");
        GameTypeButton = OptionsDialog.GetNodeOrNull<OptionButton>("MarginContainer/VBoxContainer/GameTypeContainer/GameTypeButton");
        GameOptionButton = OptionsDialog.GetNodeOrNull<OptionButton>("MarginContainer/VBoxContainer/GameOptionContainer/GameOptionButton");
        GameManager.Load();
        if (GameManager.PlayerManager.IsFirstPersonStyle)
        {
            GameOptionButton.Selected = 1;
        }
        if (GameManager.PlayerManager.IsEndless)
        {
            GameTypeButton.Selected = 1;
        }
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (OptionsDialog.Visible)
        {
            return;
        }
        if (inputEvent.IsActionPressed("num1"))
        {
            if (GameManager.PlayerManager.IsFirstPersonStyle)
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
                OptionsDialog.Popup_();
            }
        }
        if (inputEvent.IsActionPressed("num9"))
        {
            GameManager.Save();
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

    private void _on_GameOptionButton_item_selected(int selected)
    {
        if (selected == 0)
        {
            GameManager.SetGameOption(false);
        }
        else
        {
            GameManager.SetGameOption(true);
        }
    }

    private void _on_OptionsDialog_popup_hide()
    {
        GameManager.Save();
    }
}
