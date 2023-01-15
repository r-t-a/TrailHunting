using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TrailHunting.Scripts;
using TrailHunting.Scripts.Enums;
using TrailHunting.Scripts.FirstPersonScripts.Entities;
using TrailHunting.Scripts.Helpers;
using TrailHunting.Scripts.Managers;

public class FirstPersonStart : Node2D
{
    #region Exports
    [Export]
    protected NodePath AmmoNodePath;
    [Export]
    protected NodePath ReloadButtonNodePath;
    [Export]
    protected NodePath SpawnTimerNodePath;
    [Export]
    protected NodePath GameTimerNodePath;
    [Export]
    protected NodePath EndButtonNodePath;
    [Export]
    protected NodePath DisplayTimeNodePath;
    [Export]
    protected NodePath FirearmSpriteNodePath;
    #endregion

    #region Properties
    public int SmallGameCounter;
    public int MediumGameCounter;
    public int MedLargeGameCounter;
    public int LargeGameCounter;

    private AnimatedSprite levelBackground;
    private AnimatedSprite firearmSprite;
    private GridContainer ammoContainer;
    private TextureButton reloadButton;
    private Timer spawnTimer;
    private Timer gameTimer;
    private Button endButton;
    private Label displayTimer;

    private PackedScene bulletDisplay;

    private PackedScene desert;
    private PackedScene plains;
    private PackedScene river;
    private PackedScene woods;

    private PackedScene goose;
    private PackedScene duck;
    private PackedScene bear;
    private PackedScene buffalo;
    private PackedScene elk;
    private PackedScene caribou;
    private PackedScene deer;
    private PackedScene rabbit;
    private PackedScene squirrel;

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
        GameManager.PlayerManager.NeedsToReload = false;
        GameManager.PlayerManager.HasAmmo = true;

        spawnTimer = GetNodeOrNull<Timer>(SpawnTimerNodePath);
        gameTimer = GetNodeOrNull<Timer>(GameTimerNodePath);
        ammoContainer = GetNodeOrNull<GridContainer>(AmmoNodePath);
        reloadButton = GetNodeOrNull<TextureButton>(ReloadButtonNodePath);
        endButton = GetNodeOrNull<Button>(EndButtonNodePath);
        displayTimer = GetNodeOrNull<Label>(DisplayTimeNodePath);
        firearmSprite = GetNodeOrNull<AnimatedSprite>(FirearmSpriteNodePath);

        bulletDisplay = (PackedScene)ResourceLoader.Load("res://Scenes/UI/BulletDisplay.tscn");

        desert = (PackedScene)ResourceLoader.Load(Constants.Desert);
        plains = (PackedScene)ResourceLoader.Load(Constants.Plains);
        river = (PackedScene)ResourceLoader.Load(Constants.River);
        woods = (PackedScene)ResourceLoader.Load(Constants.Woods);

        goose = (PackedScene)ResourceLoader.Load(Constants.FirstPersonGoose);
        duck = (PackedScene)ResourceLoader.Load(Constants.FirstPersonDuck);
        bear = (PackedScene)ResourceLoader.Load(Constants.FirstPersonBear);
        buffalo = (PackedScene)ResourceLoader.Load(Constants.FirstPersonBuffalo);
        elk = (PackedScene)ResourceLoader.Load(Constants.FirstPersonElk);
        caribou = (PackedScene)ResourceLoader.Load(Constants.FirstPersonCaribou);
        deer = (PackedScene)ResourceLoader.Load(Constants.FirstPersonDeer);
        rabbit = (PackedScene)ResourceLoader.Load(Constants.FirstPersonRabbit);
        squirrel = (PackedScene)ResourceLoader.Load(Constants.FirstPersonSquirrel);

        if (GameManager.PlayerManager.IsEndless)
        {
            endButton.Visible = true;
            displayTimer.Visible = false;
            gameTimer.Stop();
        }
        else
        {
            endButton.Visible = false;
            displayTimer.Visible = true;
            gameTimer.Start();
        }

        SetWeaponAndAmmo();
        BuildLevel();
    }

    public override void _Process(float delta)
    {
        if (displayTimer.Visible)
        {
            displayTimer.Text = Mathf.FloorToInt(gameTimer.TimeLeft).ToString();
        }
    }
    #endregion

    #region Events
    public void _on_BackgroundArea_input_event(Node viewport, InputEvent inputEvent, int shape_idx)
    {
        if (inputEvent.IsActionPressed(Constants.Shoot) && GameManager.PlayerManager.CanShoot())
        {
            UpdateWeaponAndAmmo();
            ScatterAnimals();
        }
    }

    private void _on_GameTimer_timeout()
    {
        spawnTimer.Stop();
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
        spawnTimer.Stop();
        GameManager.BuildFirstPersonResultsDialog(SmallGameCounter, MediumGameCounter, MedLargeGameCounter, LargeGameCounter);
        GameManager.ResultsDialog.Show();
    }

    private void _on_Goose_SmallGameDead()
    {
        GameManager.UpdatePlayerStats(true, Animals.Goose);
        SmallGameCounter += 1;
    }

    private void _on_Duck_SmallGameDead()
    {
        GameManager.UpdatePlayerStats(true, Animals.Duck);
        SmallGameCounter += 1;
    }

    private void _on_Bear_MediumLargeGameDead()
    {
        GameManager.UpdatePlayerStats(true, Animals.Bear);
        MedLargeGameCounter += 1;
    }

    private void _on_Buffalo_LargeGameDead()
    {
        GameManager.UpdatePlayerStats(true, Animals.Buffalo);
        LargeGameCounter += 1;
    }

    private void _on_Elk_MediumLargeGameDead()
    {
        GameManager.UpdatePlayerStats(true, Animals.Elk);
        MedLargeGameCounter += 1;
    }

    private void _on_Caribou_MediumGameDead()
    {
        GameManager.UpdatePlayerStats(true, Animals.Caribou);
        MediumGameCounter += 1;
    }

    private void _on_Deer_MediumGameDead()
    {
        GameManager.UpdatePlayerStats(true, Animals.Buck);
        MediumGameCounter += 1;
    }

    private void _on_Rabbit_SmallGameDead()
    {
        GameManager.UpdatePlayerStats(true, Animals.Rabbit);
        SmallGameCounter += 1;
    }

    private void _on_Squirrel_SmallGameDead()
    {
        GameManager.UpdatePlayerStats(true, Animals.Squirrel);
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
            reloadButton.Pressed = false;
            GameManager.PlayerManager.NeedsToReload = false;
        }
    }

    private void _on_Move_button_up()
    {
        CleanupOnMove();
        levelBackground.Frame = levelBackground.Frame == 0 ? 1 : 0;
        PopulateBlockers();
        SpawnAirAnimals();
        SpawnGroundAnimals();
    }

    private void _on_SpawnTimer_timeout()
    {
        SpawnAirAnimals();
        RespawnGroundAnimals();
    }
    #endregion

    #region Methods
    private void SetWeaponAndAmmo()
    {
        ammoCount = GameManager.PlayerManager.FirearmType.FirearmTypeToAmmo();
        switch (GameManager.PlayerManager.FirearmType)
        {
            default:
            case FirearmsType.Flintlock:
                firearmSprite.Play("Flintlock");
                break;
            case FirearmsType.Repeating:
                firearmSprite.Play("Repeater");
                break;
            case FirearmsType.Pistol:
                firearmSprite.Play("Pistol");
                break;
        }
        firearmSprite.Playing = false;
        for(int i = 1; i <= ammoCount; i++)
        {
            var bullet = (ColorRect)bulletDisplay.Instance();
            ammoContainer.AddChild(bullet);
        }
    }

    private void UpdateWeaponAndAmmo()
    {
        if (!GameManager.PlayerManager.HasAmmo)
        {
            return;
        }
        var ammoTexture = ammoContainer.GetChildren().OfType<ColorRect>();
        foreach (var ammo in ammoTexture)
        {
            var isready = (bool)ammo.Call(Constants.GetAmmoAvailable);
            if (isready)
            {
                ammo.Call(Constants.BulletUsed);
                break;
            }
        }
        ammoCount -= 1;
        if (ammoCount == 0)
        {
            reloadButton.Disabled = true;
            GameManager.PlayerManager.NeedsToReload = false;
            GameManager.PlayerManager.HasAmmo = false;
        }
        else
        {
            if (GameManager.PlayerManager.FirearmType == FirearmsType.Repeating)
            {
                if (ammoCount % 5 == 0)
                {
                    reloadButton.Pressed = true;
                    GameManager.PlayerManager.NeedsToReload = true;
                }
                else
                {
                    reloadButton.Pressed = false;
                    GameManager.PlayerManager.NeedsToReload = false;
                }
            }
            else
            {
                reloadButton.Pressed = true;
                GameManager.PlayerManager.NeedsToReload = true;
            }
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
                var desertMap = (AnimatedSprite)desert.Instance();
                AddChild(desertMap);
                levelBackground = desertMap;
                break;
            case 1:
                var plainsMap = (AnimatedSprite)plains.Instance();
                AddChild(plainsMap);
                levelBackground = plainsMap;
                break;
            case 2:
                var riverMap = (AnimatedSprite)river.Instance();
                AddChild(riverMap);
                levelBackground = riverMap;
                break;
            case 3:
                var woodsMap = (AnimatedSprite)woods.Instance();
                AddChild(woodsMap);
                levelBackground = woodsMap;
                break;
        }

        PopulateBlockers();
        levelBackground.Playing = false;
        airSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("AirSpawns/AirSpawn1"));
        airSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("AirSpawns/AirSpawn2"));
        airSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("AirSpawns/AirSpawn3"));
        airSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("AirSpawns/AirSpawn4"));
        airSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("AirSpawns/AirSpawn5"));
        airSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("AirSpawns/AirSpawn6"));
        groundSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn1"));
        groundSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn2"));
        groundSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn3"));
        groundSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn4"));
        groundSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn5"));
        groundSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn6"));
        groundSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn7"));
        groundSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn8"));
        groundSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("GroundSpawns/GroundSpawn9"));
        timedSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("TimedSpawns/TimedSpawn1"));
        timedSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("TimedSpawns/TimedSpawn2"));
        timedSpawns.Add(levelBackground.GetNodeOrNull<Area2D>("TimedSpawns/TimedSpawn3"));
    }

    private void SpawnAirAnimals()
    {
        var numberToSpawn = new Random().Next(1, 5);
        var gooseChance = new Random().NextDouble();
        var spawn = GameManager.GetAirSpawnPoint();
        var spawnPoint = airSpawns.FirstOrDefault(x => x.Name == spawn);
        for (int i = 0; i < numberToSpawn; i++)
        {
            var animal = gooseChance > 0.5 ? (KinematicBody2D)goose.Instance() : (KinematicBody2D)duck.Instance();
            animal.Position = spawnPoint.Position + new Vector2(i * 20, i * 30);
            AddChild(animal);
            if (spawnPoint.Name.Contains("1") || spawnPoint.Name.Contains("2") || spawnPoint.Name.Contains("3"))
            {
                animal.Call(Constants.SetAirMotion, Vector2.Right);
                animal.ZIndex = 1;
            }
            else
            {
                animal.Call(Constants.SetAirMotion, Vector2.Left);
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
                animal.Call(Constants.SetMotion, Vector2.Zero);
            }
        }
    }

    private void RespawnGroundAnimals()
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
                animal.Call(Constants.SetMotion, Vector2.Zero);
            }
        }
    }

    private KinematicBody2D SpawnAnimal(int animal)
    {
        switch ((Animals)animal)
        {
            case Animals.Squirrel:
                return (KinematicBody2D)squirrel.Instance();
            case Animals.Rabbit:
                return (KinematicBody2D)rabbit.Instance();
            case Animals.Buck:
            case Animals.Doe:
                return levelBackground.Animation == Constants.RiverFrame || levelBackground.Animation == Constants.WoodsFrame ?
                       (KinematicBody2D)deer.Instance() : (KinematicBody2D)caribou.Instance();
            case Animals.Bear:
                return levelBackground.Animation == Constants.RiverFrame || levelBackground.Animation == Constants.WoodsFrame ?
                       (KinematicBody2D)bear.Instance() : (KinematicBody2D)rabbit.Instance();
            case Animals.Buffalo:
                return (KinematicBody2D)buffalo.Instance();
            case Animals.Caribou:
                return levelBackground.Animation == Constants.RiverFrame || levelBackground.Animation == Constants.WoodsFrame ?
                       (KinematicBody2D)deer.Instance() : (KinematicBody2D)caribou.Instance();
            case Animals.Elk:
                return levelBackground.Animation == Constants.RiverFrame || levelBackground.Animation == Constants.WoodsFrame ?
                       (KinematicBody2D)elk.Instance() : (KinematicBody2D)rabbit.Instance();
            default:
                return (KinematicBody2D)squirrel.Instance();
        }
    }

    private void ScatterAnimals()
    {
        CleanupBlockers();
        var currentSpawn = GetTree().GetNodesInGroup(Constants.Animals);
        foreach (var spawn in currentSpawn)
        {
            if (spawn is KinematicBody2D body && body is AnimalEntity animal)
            {
                if (animal.Name.ToLower().Contains("goose") || animal.Name.ToLower().Contains("duck") || animal.HP == 0)
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
        if (levelBackground.HasNode("Blockers"))
        {
            if (levelBackground.Frame == 0)
            {
                var blocker = levelBackground.GetNode<StaticBody2D>("Blockers/BlockerFrame0");
                var collision = blocker.GetChild<CollisionPolygon2D>(0);
                collision.Disabled = false;
            }
            if (levelBackground.Frame == 1)
            {
                var blocker = levelBackground.GetNode<StaticBody2D>("Blockers/BlockerFrame1");
                var collision = blocker.GetChild<CollisionPolygon2D>(0);
                collision.Disabled = false;
            }
            if (levelBackground.Frame == 2)
            {
                var blocker = levelBackground.GetNode<StaticBody2D>("Blockers/BlockerFrame2");
                var collision = blocker.GetChild<CollisionPolygon2D>(0);
                collision.Disabled = false;
            }
        }
    }

    private void CleanupOnMove()
    {
        CleanupBlockers();
        var currentSpawn = GetTree().GetNodesInGroup(Constants.Animals);
        foreach (var spawn in currentSpawn)
        {
            if (spawn is KinematicBody2D node)
            {
                node.QueueFree();
            }
        }
    }

    private void CleanupBlockers()
    {
        if (levelBackground.HasNode("Blockers"))
        {
            var blockerChildren = levelBackground.GetNode("Blockers").GetChildren();
            foreach (var blocker in blockerChildren)
            {
                if (blocker is StaticBody2D staticBody)
                {
                    var collision = (CollisionPolygon2D)staticBody.GetChild(0);
                    collision.Disabled = true;
                }
            }
        }
    }
    #endregion
}