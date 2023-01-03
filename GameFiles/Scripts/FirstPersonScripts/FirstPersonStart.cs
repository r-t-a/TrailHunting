using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TrailHunting.Scripts.Enums;
using TrailHunting.Scripts.FirstPersonScripts.Entities;
using TrailHunting.Scripts.Managers;

public class FirstPersonStart : Node2D
{
    #region Properties
    public int SmallGameCounter;
    public int MediumGameCounter;
    public int MedLargeGameCounter;
    public int LargeGameCounter;

    public AnimatedSprite LevelBackground;
    public GridContainer AmmoContainer;
    public TextureButton ReloadButton;
    public Timer SpawnTimer;
    public Timer GameTimer;
    public Button EndButton;
    public Label DisplayTimer;

    public PackedScene Desert;
    public PackedScene Plains;
    public PackedScene River;
    public PackedScene Woods;

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
    private List<Area2D> timedSpawns = new List<Area2D>();
    private int ammoCount;
    #endregion

    #region Overrides
    public override void _Ready()
    {
        var reticle = ResourceLoader.Load("res://Images/UI/reticle.png");
        Input.SetCustomMouseCursor(reticle, hotspot: new Vector2(8, 8));

        SpawnTimer = GetNodeOrNull<Timer>("SpawnTimer");
        GameTimer = GetNodeOrNull<Timer>("GameTimer");
        AmmoContainer = GetNodeOrNull<GridContainer>("CanvasLayer/Background/MarginContainer/HBoxContainer/GridContainer");
        ReloadButton = GetNodeOrNull<TextureButton>("CanvasLayer/Background/MarginContainer/HBoxContainer/Reload");
        EndButton = GetNodeOrNull<Button>("CanvasLayer/Background/MarginContainer/HBoxContainer/End");
        DisplayTimer = GetNodeOrNull<Label>("CanvasLayer/Background/MarginContainer/HBoxContainer/DisplayTimer");
        GameManager.PlayerManager.NeedsToReload = false;
        GameManager.PlayerManager.HasAmmo = true;

        Desert = (PackedScene)ResourceLoader.Load("res://Scenes/Levels/Desert.tscn");
        Plains = (PackedScene)ResourceLoader.Load("res://Scenes/Levels/Plains.tscn");
        River = (PackedScene)ResourceLoader.Load("res://Scenes/Levels/River.tscn");
        Woods = (PackedScene)ResourceLoader.Load("res://Scenes/Levels/Woods.tscn");

        Goose = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Goose.tscn");
        Duck = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Duck.tscn");
        Bear = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Bear.tscn");
        Buffalo = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Buffalo.tscn");
        Elk = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Elk.tscn");
        Caribou = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Caribou.tscn");
        Deer = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Deer.tscn");
        Rabbit = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Rabbit.tscn");
        Squirrel = (PackedScene)ResourceLoader.Load("res://Scenes/Animals/FirstPerson/Squirrel.tscn");

        if (GameManager.PlayerManager.IsEndless)
        {
            EndButton.Visible = true;
            DisplayTimer.Visible = false;
            GameTimer.Stop();
        }
        else
        {
            EndButton.Visible = false;
            DisplayTimer.Visible = true;
            GameTimer.Start();
        }

        ammoCount = 20; //TODO other weapons, get ammo
        BuildLevel();
    }

    public override void _Process(float delta)
    {
        if (DisplayTimer.Visible)
        {
            DisplayTimer.Text = Mathf.FloorToInt(GameTimer.TimeLeft).ToString();
        }
    }
    #endregion

    #region Events
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

    private void _on_End_button_up()
    {
        SpawnTimer.Stop();
        GameManager.BuildFirstPersonResultsDialog(SmallGameCounter, MediumGameCounter, MedLargeGameCounter, LargeGameCounter);
        GameManager.ResultsDialog.Show();
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
        if (!GameManager.PlayerManager.HasAmmo)
        {
            return;
        }

        if (GameManager.PlayerManager.NeedsToReload)
        {
            ReloadButton.Pressed = false;
            GameManager.PlayerManager.NeedsToReload = false;
        }
    }

    private void _on_Move_button_up()
    {
        CleanupOnMove();
        LevelBackground.Frame = LevelBackground.Frame == 0 ? 1 : 0;
        PopulateBlockers();
        SpawnAirAnimals();
        SpawnGroundAnimals();
    }

    private void _on_SpawnTimer_timeout()
    {
        SpawnAirAnimals();
        ReSpawnGroundAnimals();
    }
    #endregion

    #region Methods
    private void UpdateWeaponAndAmmo()
    {
        if (!GameManager.PlayerManager.HasAmmo)
        {
            return;
        }
        var ammoTexture = AmmoContainer.GetChildren().OfType<ColorRect>();
        foreach (var ammo in ammoTexture)
        {
            var isready = (bool)ammo.Call("GetAmmoAvailable");
            if (isready)
            {
                ammo.Call("BulletUsed");
                break;
            }
        }
        ammoCount -= 1;
        if (ammoCount == 0)
        {
            ReloadButton.Disabled = true;
            GameManager.PlayerManager.NeedsToReload = false;
            GameManager.PlayerManager.HasAmmo = false;
        }
        else
        {
            ReloadButton.Pressed = true;
            GameManager.PlayerManager.NeedsToReload = true;
        }
    }

    private void BuildLevel()
    {
        BuildMap();
        SpawnAirAnimals();
        SpawnGroundAnimals();
    }

    private void BuildMap()
    {
        var pickone = new Random().Next(4);
        switch (pickone)
        {
            case 0:
                var desert = (AnimatedSprite)Desert.Instance();
                AddChild(desert);
                LevelBackground = desert;
                break;
            case 1:
                var plains = (AnimatedSprite)Plains.Instance();
                AddChild(plains);
                LevelBackground = plains;
                break;
            case 2:
                var river = (AnimatedSprite)River.Instance();
                AddChild(river);
                LevelBackground = river;
                break;
            case 3:
                var woods = (AnimatedSprite)Woods.Instance();
                AddChild(woods);
                LevelBackground = woods;
                break;
        }

        PopulateBlockers();
        LevelBackground.Playing = false;
        airSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("AirSpawns/AirSpawn1"));
        airSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("AirSpawns/AirSpawn2"));
        airSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("AirSpawns/AirSpawn3"));
        airSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("AirSpawns/AirSpawn4"));
        airSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("AirSpawns/AirSpawn5"));
        airSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("AirSpawns/AirSpawn6"));
        groundSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn1"));
        groundSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn2"));
        groundSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn3"));
        groundSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn4"));
        groundSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn5"));
        groundSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn6"));
        groundSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn7"));
        groundSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn8"));
        groundSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn9"));
        timedSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("TimedSpawns/TimedSpawn1"));
        timedSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("TimedSpawns/TimedSpawn2"));
        timedSpawns.Add(LevelBackground.GetNodeOrNull<Area2D>("TimedSpawns/TimedSpawn3"));
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
            animal.Position = spawnPoint.Position + new Vector2(i * 20, i * 30);
            AddChild(animal);
            if (spawnPoint.Name.Contains("1") || spawnPoint.Name.Contains("2") || spawnPoint.Name.Contains("3"))
            {
                animal.Call("SetAirMotion", Vector2.Right);
                animal.ZIndex = 1;
            }
            else
            {
                animal.Call("SetAirMotion", Vector2.Left);
                animal.ZIndex = 2;
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
            if (spawnPoint != null)
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

    private void ReSpawnGroundAnimals()
    {
        var numberToSpawn = new Random().Next(1, 3);
        for (int i = 0; i < numberToSpawn; i++)
        {
            var randomAnimalSpawn = new Random().Next(0, 8);
            var spawn = GameManager.GetTimedSpawnPoint();
            var spawnPoint = groundSpawns.FirstOrDefault(x => x.Name == spawn);
            if (spawnPoint != null)
            {
                var animal = SpawnAnimal(randomAnimalSpawn);
                if (spawnPoint.Name.Contains("1"))
                {
                    animal.ZIndex = 1;
                }
                else if (spawnPoint.Name.Contains("2"))
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
                return LevelBackground.Animation == "Mountains" || LevelBackground.Animation == "Woods" ?
                       (KinematicBody2D)Deer.Instance() : (KinematicBody2D)Caribou.Instance();
            case Animals.Bear:
                return LevelBackground.Animation == "Mountains" || LevelBackground.Animation == "Woods" ?
                       (KinematicBody2D)Bear.Instance() : (KinematicBody2D)Rabbit.Instance();
            case Animals.Buffalo:
                return (KinematicBody2D)Buffalo.Instance();
            case Animals.Caribou:
                return LevelBackground.Animation == "Mountains" || LevelBackground.Animation == "Woods" ?
                       (KinematicBody2D)Deer.Instance() : (KinematicBody2D)Caribou.Instance();
            case Animals.Elk:
                return LevelBackground.Animation == "Mountains" || LevelBackground.Animation == "Woods" ?
                       (KinematicBody2D)Elk.Instance() : (KinematicBody2D)Rabbit.Instance();
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

    private void PopulateBlockers()
    {
        if (LevelBackground.HasNode("Blockers"))
        {
            if (LevelBackground.Frame == 0)
            {
                var blocker = LevelBackground.GetNode<StaticBody2D>("Blockers/BlockerFrame0");
                var collision = blocker.GetChild<CollisionPolygon2D>(0);
                collision.Disabled = false;
            }
            if (LevelBackground.Frame == 1)
            {
                var blocker = LevelBackground.GetNode<StaticBody2D>("Blockers/BlockerFrame1");
                var collision = blocker.GetChild<CollisionPolygon2D>(0);
                collision.Disabled = false;
            }
            if (LevelBackground.Frame == 2)
            {
                var blocker = LevelBackground.GetNode<StaticBody2D>("Blockers/BlockerFrame2");
                var collision = blocker.GetChild<CollisionPolygon2D>(0);
                collision.Disabled = false;
            }
        }
    }

    private void CleanupOnMove()
    {
        if (LevelBackground.HasNode("Blockers"))
        {
            var blockerChildren = LevelBackground.GetNode("Blockers").GetChildren();
            foreach (var blocker in blockerChildren)
            {
                if (blocker is StaticBody2D staticBody)
                {
                    var collision = (CollisionPolygon2D)staticBody.GetChild(0);
                    collision.Disabled = true;
                }
            }
        }

        var currentSpawn = GetTree().GetNodesInGroup("Animals");
        foreach (var spawn in currentSpawn)
        {
            if (spawn is KinematicBody2D node)
            {
                node.QueueFree();
            }
        }
    }
    #endregion
}