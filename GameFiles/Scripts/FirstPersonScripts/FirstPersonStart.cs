using Godot;
using System;
using System.Linq;
using TrailHunting.Scripts.Helpers;
using TrailHunting.Scripts.Managers;

public class FirstPersonStart : Node2D
{
    public Sprite LevelBackground;
    public GridContainer AmmoContainer;
    public TextureButton ReloadButton;

    private int currentLevel;
    private bool canShoot;

    public override void _Ready()
    {
        var reticle = ResourceLoader.Load("res://Images/UI/reticle.png");
        Input.SetCustomMouseCursor(reticle);
        
        LevelBackground = GetNode<Sprite>("LevelImage");
        AmmoContainer = GetNode<GridContainer>("CanvasLayer/Background/MarginContainer/HBoxContainer/GridContainer");
        ReloadButton = GetNode<TextureButton>("CanvasLayer/Background/MarginContainer/HBoxContainer/Reload");
        ReloadButton.Disabled = true;

        BuildMap();
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("shoot") && canShoot)
        {
            var ammoTexture = AmmoContainer.GetChildren().OfType<TextureRect>().Where(x => x.Visible);
            if (!ammoTexture.Any())
            {
                return;
            }
            var lastNode = AmmoContainer.GetNode<TextureRect>(ammoTexture.LastOrDefault().GetPath());
            AmmoContainer.RemoveChild(lastNode);

            ReloadButton.Disabled = false;
            ReloadButton.Pressed = true;
            canShoot = false;

            //check if animal hit
        }
    }

    private void _on_Reload_pressed()
    {
        if (ReloadButton.Pressed)
        {
            ReloadButton.Disabled = true;
            ReloadButton.Pressed = false;
            canShoot = true;
        }
    }

    private void _on_Move_button_up()
    {
        BuildMap();
    }

    private void _on_MarginContainer_mouse_entered()
    {
        canShoot = false;
    }

    private void _on_LevelImage_mouse_entered()
    {
        canShoot = !ReloadButton.Pressed;
    }

    private void _on_Exit_button_up()
    {
        Input.SetCustomMouseCursor(null);
        GameManager.End();
    }

    //Call GDScript to change Texture, C# not supported when exporting?
    private void BuildMap()
    {
        var randomMap = new Random().Next(5);
        if (currentLevel == randomMap)
        {
            BuildMap();
        }
        currentLevel = randomMap;
        //LevelBackground.Call("loadNewBackground", randomMap);
    }
}
