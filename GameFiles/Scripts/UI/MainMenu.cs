using Godot;
using TrailHunting.Scripts;
using TrailHunting.Scripts.Enums;
using TrailHunting.Scripts.Managers;

public class MainMenu : Control
{
    #region Exports
    [Export]
    protected NodePath WindowsDialogNodePath;
    [Export]
    protected NodePath GameTypeNodePath;
    [Export]
    protected NodePath GameOptionNodePath;
    [Export]
    protected NodePath FirearmNodePath;
    [Export]
    protected NodePath PanelAnimationNodePath;
    [Export]
    protected NodePath GeeseCountNodePath;
    [Export]
    protected NodePath DuckCountNodePath;
    [Export]
    protected NodePath RabbitCountNodePath;
    [Export]
    protected NodePath SquirrelCountNodePath;
    [Export]
    protected NodePath DoeCountNodePath;
    [Export]
    protected NodePath BuckCountNodePath;
    [Export]
    protected NodePath CaribouCoundNodePath;
    [Export]
    protected NodePath ElkCountNodePath;
    [Export]
    protected NodePath BearCountNodePath;
    [Export]
    protected NodePath BuffaloCountNodePath;
    [Export]
    protected NodePath FlintlockCountNodePath;
    [Export]
    protected NodePath RepeaterCountNodePath;
    [Export]
    protected NodePath PistolCountNodePath;
    #endregion

    #region Properties
    private WindowDialog optionsDialog;
    private AnimationPlayer panelAnimation;
    private OptionButton gameTypeButton;
    private OptionButton gameOptionButton;
    private OptionButton firearmButton;
    private Label geeseCountLabel;
    private Label duckCountLabel;
    private Label rabbitCountLabel;
    private Label squirrelCountLabel;
    private Label doeCountLabel;
    private Label buckCountLabel;
    private Label caribouCountLabel;
    private Label elkCountLabel;
    private Label bearCountLabel;
    private Label buffaloCountLabel;
    private Label flintLockCountLabel;
    private Label repeaterCountLabel;
    private Label pistolCountLabel;
    #endregion

    #region Overrides
    public override void _Ready()
    {
        optionsDialog = GetNodeOrNull<WindowDialog>(WindowsDialogNodePath);
        panelAnimation = GetNodeOrNull<AnimationPlayer>(PanelAnimationNodePath);
        gameTypeButton = GetNodeOrNull<OptionButton>(GameTypeNodePath);
        gameOptionButton = GetNodeOrNull<OptionButton>(GameOptionNodePath);
        firearmButton = GetNodeOrNull<OptionButton>(FirearmNodePath);
        geeseCountLabel = GetNodeOrNull<Label>(GeeseCountNodePath);
        duckCountLabel = GetNodeOrNull<Label>(DuckCountNodePath);
        rabbitCountLabel = GetNodeOrNull<Label>(RabbitCountNodePath);
        squirrelCountLabel = GetNodeOrNull<Label>(SquirrelCountNodePath);
        doeCountLabel = GetNodeOrNull<Label>(DoeCountNodePath);
        buckCountLabel = GetNodeOrNull<Label>(BuckCountNodePath);
        caribouCountLabel = GetNodeOrNull<Label>(CaribouCoundNodePath);
        elkCountLabel = GetNodeOrNull<Label>(ElkCountNodePath);
        bearCountLabel = GetNodeOrNull<Label>(BearCountNodePath);
        buffaloCountLabel = GetNodeOrNull<Label>(BuffaloCountNodePath);
        flintLockCountLabel = GetNodeOrNull<Label>(FlintlockCountNodePath);
        repeaterCountLabel = GetNodeOrNull<Label>(RepeaterCountNodePath);
        pistolCountLabel = GetNodeOrNull<Label>(PistolCountNodePath);

        GameManager.Load();
        GameManager.LoadVariables();
        if (GameManager.PlayerManager.IsFirstPersonStyle)
        {
            gameOptionButton.Selected = 1;
        }
        if (GameManager.PlayerManager.IsEndless)
        {
            gameTypeButton.Selected = 1;
        }
        firearmButton.Selected = (int)GameManager.PlayerManager.FirearmType;
        panelAnimation.Play("Default");
        SetPlayerStats();
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
    #endregion

    #region Events
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

    private void _on_GunTypeButton_item_selected(int selected)
    {
        GameManager.PlayerManager.FirearmType = (FirearmsType)selected;
    }

    private void _on_StatsButton_button_up()
    {
        optionsDialog.Hide();
        panelAnimation.Play("Open");
    }

    private void _on_ClosePanelButton_button_up()
    {
        panelAnimation.Play("Close");
    }

    private void _on_OptionsDialog_popup_hide()
    {
        GameManager.Save();
    }
    #endregion

    #region Methods
    private void SetPlayerStats()
    {
        geeseCountLabel.Text = GameManager.PlayerManager.GooseTotal.ToString();
        duckCountLabel.Text = GameManager.PlayerManager.DuckTotal.ToString();
        rabbitCountLabel.Text = GameManager.PlayerManager.RabbitTotal.ToString();
        squirrelCountLabel.Text = GameManager.PlayerManager.SquirrelTotal.ToString();
        doeCountLabel.Text = GameManager.PlayerManager.DoeTotal.ToString();
        buckCountLabel.Text = GameManager.PlayerManager.BuckTotal.ToString();
        caribouCountLabel.Text = GameManager.PlayerManager.CaribouTotal.ToString();
        elkCountLabel.Text = GameManager.PlayerManager.ElkTotal.ToString();
        bearCountLabel.Text = GameManager.PlayerManager.BearTotal.ToString();
        buffaloCountLabel.Text = GameManager.PlayerManager.BuffaloTotal.ToString();
        flintLockCountLabel.Text = GameManager.PlayerManager.FlintlockTotal.ToString();
        repeaterCountLabel.Text = GameManager.PlayerManager.RepeaterTotal.ToString();
        pistolCountLabel.Text = GameManager.PlayerManager.PistolTotal.ToString();
    }
    #endregion
}
