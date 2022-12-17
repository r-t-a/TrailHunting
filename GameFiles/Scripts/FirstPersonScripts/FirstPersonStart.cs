using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TrailHunting.Scripts.Helpers;
using TrailHunting.Scripts.Managers;

public class FirstPersonStart : Node2D
{
    public Sprite LevelBackground;
    public GridContainer AmmoContainer;
    public TextureButton ReloadButton;

    private Dictionary<int, string> levels = new Dictionary<int, string>();
    private string currentLevel;
    private bool canShoot;

    public override void _Ready()
    {
        var reticle = ResourceLoader.Load("res://Images/UI/reticle.png");
        Input.SetCustomMouseCursor(reticle);
        
        LevelBackground = GetNode<Sprite>("LevelImage");
        AmmoContainer = GetNode<GridContainer>("CanvasLayer/Background/MarginContainer/HBoxContainer/GridContainer");
        ReloadButton = GetNode<TextureButton>("CanvasLayer/Background/MarginContainer/HBoxContainer/Reload");
        ReloadButton.Disabled = true;

        levels.Add(0, "res://Images/UI/Plains_1.png");
        levels.Add(1, "res://Images/UI/Plains_2.png");
        levels.Add(2, "res://Images/UI/Plains_3.png");
        levels.Add(3, "res://Images/UI/Desert_1.png");
        levels.Add(4, "res://Images/UI/Desert_2.png");

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
        GameManager.End();
    }

    private void BuildMap()
    {
        currentLevel = GetRandomNext();
        if (!string.IsNullOrWhiteSpace(currentLevel))
        {
            SetBackgroundTexture(currentLevel.LoadImage());
        }
        else
        {
            currentLevel = "res://Images/UI/Plains_1.png";
            SetBackgroundTexture(currentLevel.LoadImage());
        }
    }

    private string GetRandomNext()
    {
        var tempcurrent = currentLevel;
        var randomStart = new Random().Next(levels.Count);
        currentLevel = levels.FirstOrDefault(x => x.Key == randomStart).Value;
        if (currentLevel == tempcurrent)
        {
            GetRandomNext();
        }
        return currentLevel;
    }

    private void SetBackgroundTexture(Image imageBackground)
    {
        var imageTexture = new ImageTexture();
        imageTexture.CreateFromImage(imageBackground);
        LevelBackground.Texture = imageTexture;
    }
}
