using Godot;
using TrailHunting.Scripts;

public class MainMenu : Control
{
    public VBoxContainer SecondaryButtons;

    public override void _Ready()
    {
        SecondaryButtons = GetNode<VBoxContainer>("MarginContainer/HBoxContainer/SecondaryButtons");
        SecondaryButtons.Visible = false;
    }

    private void _on_Start_button_up()
    {
        SecondaryButtons.Visible = true;
    }

    private void _on_Exit_button_up()
    {
        GetTree().Quit();
    }

    private void _on_TopDown_button_up()
    {
        GetTree().ChangeScene(Constants.TopDownStart);
    }

    private void _on_FirstPerson_button_up()
    {
        GetTree().ChangeScene(Constants.FirstPersonStart);
    }
}
