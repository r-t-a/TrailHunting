using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TrailHunting.Scripts.Enums;
using TrailHunting.Scripts.FirstPersonScripts.Entities;
using TrailHunting.Scripts.Managers;

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
    public Timer SpawnTimer;

    public PackedScene Goose;
    public PackedScene Duck;
    public PackedScene Bear;
    public PackedScene Buffalo;
    public PackedScene Elk;
    public PackedScene Caribou;
    public PackedScene Deer;
    public PackedScene Rabbit;
    public PackedScene Squirrel;

    private List<Area2D> airSpawns = new List<Area2D>();
	private List<Area2D> groundSpawns = new List<Area2D>();

	public override void _Ready()
	{
		var reticle = ResourceLoader.Load("res://Images/UI/reticle.png");
		Input.SetCustomMouseCursor(reticle, hotspot: new Vector2(8,8));
		
		LevelBackground = GetNodeOrNull<AnimatedSprite>("LevelImage");
        SpawnTimer = GetNodeOrNull<Timer>("SpawnTimer");
        AmmoContainer = GetNodeOrNull<GridContainer>("CanvasLayer/Background/MarginContainer/HBoxContainer/GridContainer");
		ReloadButton = GetNodeOrNull<TextureButton>("CanvasLayer/Background/MarginContainer/HBoxContainer/Reload");
		ReloadButton.Disabled = true;
        GameManager.PlayerManager.NeedsToReload = false;

        Goose = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Goose.tscn");
        Duck = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Duck.tscn");
        Bear = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Bear.tscn");
        Buffalo = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Buffalo.tscn");
        Elk = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Elk.tscn");
        Caribou = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Caribou.tscn");
        Deer = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Deer.tscn");
        Rabbit = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Rabbit.tscn");
        Squirrel = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Squirrel.tscn");

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

        LevelBackground.Playing = false; // Player controls the Frame
        BuildLevel();
	}

    public void _on_BackgroundArea_input_event(Node viewport, InputEvent inputEvent, int shape_idx)
    {
        if (inputEvent.IsActionPressed("shoot") && GameManager.PlayerManager.CanShoot())
        {
            UpdateWeaponAndAmmo();
            ScatterAnimals();
        }
    }

    private void _on_GameTimer_timeout()
    {
        SpawnTimer.Stop();
        GameManager.BuildFirstPersonResultsDialog(SmallGameCounter, MediumGameCounter, MedLargeGameCounter, LargeGameCounter);
        GameManager.ResultsDialog.Show();
    }

    private void _on_AcceptDialog_modal_closed()
    {
        GameManager.ResultsDialog.Hide();
        GameManager.End();
    }

    private void _on_AcceptDialog_confirmed()
    {
        GameManager.ResultsDialog.Hide();
        GameManager.End();
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
        GameManager.PlayerManager.NeedsToReload = true;
    }

    private void BuildLevel()
	{
		BuildMap();
        SpawnAirAnimals();
		SpawnGroundAnimals();
    }

	private void _on_Ground_body_entered(Node node)
	{
        if (node is KinematicBody2D body && (body.Name.Contains("Goose") ||
            body.Name.Contains("Duck")) && (body as AnimalEntity).HP == 0)
        {
            (body as AnimalEntity).Motion = Vector2.Zero;
        }
    }

    private void _on_Goose_SmallGameDead()
    {
		SmallGameCounter += 1;
    }

    private void _on_Duck_SmallGameDead()
    {
        SmallGameCounter += 1;
    }

    private void _on_Bear_MediumLargeGameDead()
    {
        MedLargeGameCounter += 1;
    }

    private void _on_Buffalo_LargeGameDead()
    {
        LargeGameCounter += 1;
    }

    private void _on_Elk_MediumLargeGameDead()
    {
        MedLargeGameCounter += 1;
    }

    private void _on_Caribou_MediumGameDead()
    {
        MediumGameCounter += 1;
    }

    private void _on_Deer_MediumGameDead()
    {
        MediumGameCounter += 1;
    }

    private void _on_Rabbit_SmallGameDead()
    {
        SmallGameCounter += 1;
    }

    private void _on_Squirrel_SmallGameDead()
    {
        SmallGameCounter += 1;
    }

    private void _on_Reload_pressed()
	{
		if (GameManager.PlayerManager.NeedsToReload)
		{
			ReloadButton.Disabled = true;
			ReloadButton.Pressed = false;
            GameManager.PlayerManager.NeedsToReload = false;
		}
	}

	private void _on_Move_button_up()
	{
        CleanupOnMove();
        LevelBackground.Frame = LevelBackground.Frame == 0 ? 1 : 0;
        LevelBackground.Playing = false;
        SpawnAirAnimals();
        SpawnGroundAnimals();
    }

    private void _on_SpawnTimer_timeout()
    {
        SpawnAirAnimals();
        SpawnGroundAnimals();
    }

    private void _on_Exit_button_up()
	{
		Input.SetCustomMouseCursor(null);
		GameManager.End();
	}

	private void BuildMap()
	{
		LevelBackground.Play(GameManager.BuildFirstPersonLevel());
		LevelBackground.Playing = false;
	}

	private void SpawnAirAnimals()
	{
        var numberToSpawn = new Random().Next(1, 5);
        var gooseChance = new Random().NextDouble();
        var spawn = GameManager.GetAirSpawnPoint();
        var spawnPoint = airSpawns.FirstOrDefault(x => x.Name == spawn);
        for (int i = 0; i < numberToSpawn; i++)
        {
            var animal = gooseChance > 0.5 ? (KinematicBody2D)Goose.Instance() : (KinematicBody2D)Duck.Instance();
            animal.Position = spawnPoint.Position + new Vector2(i * 20, i * 50);
            AddChild(animal);
            if (spawnPoint.Name.Contains("1") || spawnPoint.Name.Contains("2") || spawnPoint.Name.Contains("3"))
            {
                animal.Call("SetAirMotion", Vector2.Right);
            }
            else
            {
                animal.Call("SetAirMotion", Vector2.Left);
            }
        }
    }

	private void SpawnGroundAnimals()
	{
        var numberToSpawn = new Random().Next(1, 5);
        for (int i = 0; i < numberToSpawn; i++)
        {
            var randomAnimalSpawn = new Random().Next(0, 8);
            var spawn = GameManager.GetGroundSpawnPoint();
            var spawnPoint = groundSpawns.FirstOrDefault(x => x.Name == spawn);
            if (spawnPoint != null )
            {
                var animal = SpawnAnimal(randomAnimalSpawn);
                if (spawnPoint.Name.Contains("1") || spawnPoint.Name.Contains("2") || spawnPoint.Name.Contains("3"))
                {
                    animal.ZIndex = 1;
                }
                else if (spawnPoint.Name.Contains("4") || spawnPoint.Name.Contains("5") || spawnPoint.Name.Contains("6"))
                {
                    animal.ZIndex = 2;
                    animal.Scale = new Vector2((float)1.4, (float)1.4);
                }
                else
                {
                    animal.ZIndex = 3;
                    animal.Scale = new Vector2((float)1.6, (float)1.6);
                }
                animal.Position = spawnPoint.Position;
                AddChild(animal);
                animal.Call("SetMotion", Vector2.Zero);
            }
        }
    }

    private KinematicBody2D SpawnAnimal(int animal)
    {
        switch ((Animals)animal)
        {
            case Animals.Squirrel:
                return (KinematicBody2D)Squirrel.Instance();
            case Animals.Rabbit:
                return (KinematicBody2D)Rabbit.Instance();
            case Animals.Buck:
            case Animals.Doe:
                return (KinematicBody2D)Deer.Instance();
            case Animals.Bear:
                return (KinematicBody2D)Bear.Instance();
            case Animals.Buffalo:
                return (KinematicBody2D)Buffalo.Instance();
            case Animals.Caribou:
                return (KinematicBody2D)Caribou.Instance();
            case Animals.Elk:
                return (KinematicBody2D)Elk.Instance();
            default:
                return (KinematicBody2D)Squirrel.Instance();
        }
    }

    private void ScatterAnimals()
    {
        var currentSpawn = GetTree().GetNodesInGroup("Animals");
        foreach (var spawn in currentSpawn)
        {
            if (spawn is KinematicBody2D body && body is AnimalEntity animal)
            {
                if (animal.Name.Contains("Goose") || animal.Name.Contains("Duck") || animal.HP == 0)
                {
                    continue;
                }
                animal.SetMotion(Vector2.Left);
                animal.Timer.Stop();
            }
        }
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
