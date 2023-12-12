using Godot;
using System.Threading.Tasks;
using TrailHunting.Scripts;

public class Intro : Control
{
    #region Exports
    [Export]
    protected NodePath TimerNodePath { get; private set; }
    [Export]
    protected NodePath ContainerNodePath { get; private set; }
    [Export]
    protected NodePath AudioStreamPlayerNodePath { get; private set; }
    [Export]
    protected NodePath AnimationPlayerNodePath { get; private set; }
    #endregion

    #region Properties
    private Timer timer;
    private VBoxContainer container;
    private AudioStreamPlayer audioStreamPlayer;
    private AnimationPlayer animationPlayer;
    #endregion

    #region Overrides
    public override void _Ready()
    {
        timer = GetNodeOrNull<Timer>(TimerNodePath);
        container = GetNodeOrNull<VBoxContainer>(ContainerNodePath);
        audioStreamPlayer = GetNodeOrNull<AudioStreamPlayer>(AudioStreamPlayerNodePath);
        animationPlayer = GetNodeOrNull<AnimationPlayer>(AnimationPlayerNodePath);
        Setup();
    }
    #endregion

    #region Events
    private void _on_Timer_timeout() => animationPlayer.Play(Constants.PopIn);

    private async void _on_AnimationPlayer_animation_finished(string animation)
    {
        if (animation == Constants.PopIn)
        {
            audioStreamPlayer.VolumeDb = -3;
            audioStreamPlayer.Play();
            await Task.Delay(2000);
            GetTree().ChangeScene(Constants.MainMenu);
        }
    }
    #endregion

    #region Methods
    private void Setup()
    {
        container.Hide();
        if (timer.IsStopped())
            timer.Start();
    }
    #endregion
}