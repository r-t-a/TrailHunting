using Godot;
using TrailHunting.Scripts;
using TrailHunting.Scripts.Managers;

public class MainMenu : Control
{
    [Export]
    protected NodePath WindowsDialogPath;
    [Export]
    protected NodePath GameTypePath;
    [Export]
    protected NodePath GameOptionPath;

    private WindowDialog optionsDialog;
    private OptionButton gameTypeButton;
    private OptionButton gameOptionButton;

    public override void _Ready()
    {
        optionsDialog = GetNodeOrNull<WindowDialog>(WindowsDialogPath);
        gameTypeButton = GetNodeOrNull<OptionButton>(GameTypePath);
        gameOptionButton = GetNodeOrNull<OptionButton>(GameOptionPath);
        GameManager.Load();
        if (GameManager.PlayerManager.IsFirstPersonStyle)
        {
            gameOptionButton.Selected = 1;
        }
        if (GameManager.PlayerManager.IsEndless)
        {
            gameTypeButton.Selected = 1;
        }
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (optionsDialog.Visible)
        {
            return;
        }
        if (inputEvent.IsActionPressed(Constants.Num1))
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
        if (inputEvent.IsActionPressed(Constants.Num8))
        {
            if (!optionsDialog.Visible)
            {
                optionsDialog.Popup_();
            }
        }
        if (inputEvent.IsActionPressed(Constants.Num9))
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
