using Godot;
using System;
using TrailHunting.Scripts;
using TrailHunting.Scripts.Managers;

public class Results : ColorRect
{
    [Signal]
    public delegate void RestartGame();

    [Export]
    protected NodePath MeatCollectedNodePath { get; private set; }
    [Export]
    protected NodePath AccuracyNodePath { get; private set; }
    [Export]
    protected NodePath TotalScoreNodePath { get; private set; }

    private Label meatCollectedLabel;
    private Label accuracyLabel;
    private Label totalScoreLabel;

    public override void _Ready()
    {
        meatCollectedLabel = GetNodeOrNull<Label>(MeatCollectedNodePath);
        accuracyLabel = GetNodeOrNull<Label>(AccuracyNodePath);
        totalScoreLabel = GetNodeOrNull<Label>(TotalScoreNodePath);
        PauseMode = PauseModeEnum.Process;
    }

    private void _on_Exit_pressed()
    {
        GetTree().ChangeScene(Constants.MainMenu);
        GetTree().Paused = false;
    }

    private void _on_Retry_pressed()
    {
        EmitSignal(nameof(RestartGame));
    }

    public void SetResults(int small, int medium, int medLarge, int large, int totalShot, int hit, int score)
    {
        meatCollectedLabel.Text = $"{GameManager.GetTotalMeat(small, medium, medLarge, large)}";
        accuracyLabel.Text = $"{hit}/{totalShot}";
        totalScoreLabel.Text = $"{score}";
    }
}
