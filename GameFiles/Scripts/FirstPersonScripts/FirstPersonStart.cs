using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TrailHunting.Scripts.FirstPersonScripts.Entities;
using TrailHunting.Scripts.Managers;
using static Bear2;

public class FirstPersonStart : Node2D
{
    [Export]
    public int SmallGameCounter;
    [Export]
    public int MediumGameCounter;
    [Export]
    public int MedLargeGameCounter;
    [Export]
    public int LargeGameCounter;

    public AnimatedSprite LevelBackground;
	public GridContainer AmmoContainer;
	public TextureButton ReloadButton;

    public PackedScene Goose;
	public PackedScene Bear2;
    public PackedScene Duck;

    private List<Area2D> airSpawns = new List<Area2D>();
	private List<Area2D> groundSpawns = new List<Area2D>();
    private int currentLevel;
	private bool canShoot;

	public override void _Ready()
	{
		var reticle = ResourceLoader.Load("res://Images/UI/reticle.png");
		Input.SetCustomMouseCursor(reticle, hotspot: new Vector2(8,8));
		
		LevelBackground = GetNodeOrNull<AnimatedSprite>("LevelImage");
		AmmoContainer = GetNodeOrNull<GridContainer>("CanvasLayer/Background/MarginContainer/HBoxContainer/GridContainer");
		ReloadButton = GetNodeOrNull<TextureButton>("CanvasLayer/Background/MarginContainer/HBoxContainer/Reload");
		ReloadButton.Disabled = true;
        canShoot = true;

        Goose = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Goose.tscn");
        Bear2 = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Bear.tscn");
        Duck = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Duck.tscn");

        // TODO just loop this, make max const
        airSpawns.Add(GetNodeOrNull<Area2D>("AirSpawns/AirSpawn1"));
        airSpawns.Add(GetNodeOrNull<Area2D>("AirSpawns/AirSpawn2"));
        airSpawns.Add(GetNodeOrNull<Area2D>("AirSpawns/AirSpawn3"));
        airSpawns.Add(GetNodeOrNull<Area2D>("AirSpawns/AirSpawn4"));
        airSpawns.Add(GetNodeOrNull<Area2D>("AirSpawns/AirSpawn5"));
        airSpawns.Add(GetNodeOrNull<Area2D>("AirSpawns/AirSpawn6"));
		groundSpawns.Add(GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn1"));
        groundSpawns.Add(GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn2"));
        groundSpawns.Add(GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn3"));
        groundSpawns.Add(GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn4"));
        groundSpawns.Add(GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn5"));
        groundSpawns.Add(GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn6"));
        groundSpawns.Add(GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn7"));
        groundSpawns.Add(GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn8"));
        groundSpawns.Add(GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn9"));
        groundSpawns.Add(GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn10"));
        groundSpawns.Add(GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn11"));
        groundSpawns.Add(GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn12"));

        LevelBackground.Playing = false; // Player controls the Frame
        BuildLevel();
	}

    public void _on_BackgroundArea_input_event(Node viewport, InputEvent inputEvent, int shape_idx)
    {
        if (inputEvent.IsActionPressed("shoot") && canShoot)
        {
            UpdateWeaponAndAmmo();
        }
    }

    private void UpdateWeaponAndAmmo()
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
    }

    private void BuildLevel()
	{
		BuildMap();
        SpawnAirAnimals();
		SpawnGroundAnimals();
    }

	private void _on_Ground_body_entered(Node node)
	{
        if (node is KinematicBody2D body && (body.Name.StartsWith("Goose") || body.Name.StartsWith("Duck")) && (body as AnimalEntity).HP == 0)
        {
            (body as AnimalEntity).Motion = Vector2.Zero;
        }
    }

    private void _on_Goose_SmallGameDead()
    {
		SmallGameCounter += 1;
    }

    private void _on_Bear_MediumLargeGameDead()
    {
        MediumGameCounter += 1;
    }

    private void _on_Duck_SmallGameDead()
    {
        SmallGameCounter += 1;
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
        CleanupOnMove();
        LevelBackground.Frame = LevelBackground.Frame == 0 ? 1 : 0;
        LevelBackground.Playing = false;
    }

	private void _on_MarginContainer_mouse_entered()
	{
		canShoot = false;
	}

	private void _on_BackgroundArea_mouse_entered()
	{
		canShoot = !ReloadButton.Pressed;
	}

    private void _on_Exit_button_up()
	{
		Input.SetCustomMouseCursor(null);
		GameManager.End();
	}

	private void BuildMap()
	{
		var randomMap = new Random().Next(5);
		if (currentLevel == randomMap)
		{
			BuildMap();
		}
		LevelBackground.Play(GameManager.BuildFirstPersonLevel());
		LevelBackground.Playing = false;
	}

	private void SpawnAirAnimals()
	{
        // TODO Loop and % chance for certain

		var spawn = GameManager.GetAirSpawnPoint();
        var spawnPoint = airSpawns.FirstOrDefault(x => x.Name == spawn);
        spawnPoint = spawnPoint ?? GetNodeOrNull<Area2D>("AirSpawns/AirSpawn1");
        var goose = (KinematicBody2D)Goose.Instance();
        goose.Position = spawnPoint.Position;
		if (spawnPoint.Name.Contains("1") || spawnPoint.Name.Contains("2") || spawnPoint.Name.Contains("3"))
		{
			goose.Call("SetMotion", Vector2.Right);
		}
		else
		{
			goose.Call("SetMotion", Vector2.Left);
		}

        AddChild(goose);
        airSpawns.Remove(spawnPoint);
    }

	private void SpawnGroundAnimals()
	{
        // TODO Loop and % chance for certain

        var spawn = GameManager.GetGroundSpawnPoint();
        var spawnPoint = groundSpawns.FirstOrDefault(x => x.Name == spawn);
        spawnPoint = spawnPoint ?? GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn1");
        var bear = (KinematicBody2D)Bear2.Instance();
        bear.Position = spawnPoint.Position;
        if (spawnPoint.Name.Contains("1") || spawnPoint.Name.Contains("2") || spawnPoint.Name.Contains("3") ||
            spawnPoint.Name.Contains("4") || spawnPoint.Name.Contains("5") || spawnPoint.Name.Contains("6") ||
            spawnPoint.Name.Contains("7") || spawnPoint.Name.Contains("8"))
        {
            bear.Call("SetMotion", Vector2.Right);
        }
        else
        {
            bear.Call("SetMotion", Vector2.Left);
        }
        AddChild(bear);
        groundSpawns.Remove(spawnPoint);
    }


    private void CleanupOnMove()
	{
        var currentSpawn = GetTree().GetNodesInGroup("Animals");
        foreach (var spawn in currentSpawn)
        {
            if (spawn is KinematicBody2D node)
            {
                node.QueueFree();
            }
        }
    }
}
