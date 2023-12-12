using Godot;
using TrailHunting.Scripts;
using TrailHunting.Scripts.Managers;

public class MainMenu : Control
{
    #region Exports
    [Export]
    protected NodePath PanelAnimationNodePath { get; private set; }
    [Export]
    protected NodePath FocusTransparentBackgroundNodePath { get; private set; }
    [Export]
    protected NodePath GeeseCountNodePath { get; private set; }
    [Export]
    protected NodePath DuckCountNodePath { get; private set; }
    [Export]
    protected NodePath RabbitCountNodePath { get; private set; }
    [Export]
    protected NodePath SquirrelCountNodePath { get; private set; }
    [Export]
    protected NodePath DoeCountNodePath { get; private set; }
    [Export]
    protected NodePath BuckCountNodePath { get; private set; }
    [Export]
    protected NodePath CaribouCountNodePath { get; private set; }
    [Export]
    protected NodePath ElkCountNodePath { get; private set; }
    [Export]
    protected NodePath BearCountNodePath { get; private set; }
    [Export]
    protected NodePath BuffaloCountNodePath { get; private set; }
    #endregion

    #region Properties
    private AnimationPlayer panelAnimation;
    private ColorRect focusTransparentBackground;
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
    #endregion

    #region Overrides
    public override void _Ready()
    {
        panelAnimation = GetNodeOrNull<AnimationPlayer>(PanelAnimationNodePath);
        focusTransparentBackground = GetNodeOrNull<ColorRect>(FocusTransparentBackgroundNodePath);
        geeseCountLabel = GetNodeOrNull<Label>(GeeseCountNodePath);
        duckCountLabel = GetNodeOrNull<Label>(DuckCountNodePath);
        rabbitCountLabel = GetNodeOrNull<Label>(RabbitCountNodePath);
        squirrelCountLabel = GetNodeOrNull<Label>(SquirrelCountNodePath);
        doeCountLabel = GetNodeOrNull<Label>(DoeCountNodePath);
        buckCountLabel = GetNodeOrNull<Label>(BuckCountNodePath);
        caribouCountLabel = GetNodeOrNull<Label>(CaribouCountNodePath);
        elkCountLabel = GetNodeOrNull<Label>(ElkCountNodePath);
        bearCountLabel = GetNodeOrNull<Label>(BearCountNodePath);
        buffaloCountLabel = GetNodeOrNull<Label>(BuffaloCountNodePath);

        GameManager.Load();
        GameManager.LoadVariables();
        panelAnimation.Play(Constants.Default);
        focusTransparentBackground.Hide();
        SetPlayerStats();
    }
    #endregion

    #region Events
    private void _on_ModeA_pressed()
    {
        if (focusTransparentBackground.Visible) return;
        GetTree().ChangeScene(Constants.TopDownStart);
    }

    private void _on_ModeB_pressed()
    {
        if (focusTransparentBackground.Visible) return;
        GetTree().ChangeScene(Constants.FirstPersonStart);
    }

    private void _on_ClosePanelButton_button_up()
    {
        focusTransparentBackground.Hide();
        panelAnimation.Play(Constants.Close);
    }

    private void _on_Stats_pressed()
    {
        if (focusTransparentBackground.Visible) return;
        focusTransparentBackground.Show();
        panelAnimation.Play(Constants.Open);
    }

    private void _on_Exit_pressed()
    {
        if (focusTransparentBackground.Visible) return;
        GameManager.Save();
        GetTree().Quit();
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
    }
    #endregion
}